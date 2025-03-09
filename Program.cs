using Newtonsoft.Json;
using OfficeOpenXml;
using MikeRostoli.Classes;

class Program
{
    static void Main()
    {
        string jsonFilePath = @"../../../examples/config/staff_data.example.json";
        var jsonString = File.ReadAllText(jsonFilePath);

        var staffRoster = JsonConvert.DeserializeObject<StaffRoster>(jsonString);

        if (staffRoster == null || staffRoster.Staff == null || staffRoster.Config == null)
        {
            Console.WriteLine("Error: Staff data or configuration is missing or could not be loaded.");
            return;
        }

        // get config values from the JSON
        var days = staffRoster.Config.Days;
        var times = staffRoster.Config.Times;
        int minStaffPerShift = staffRoster.Config.MinStaffPerShift;

        List<Shift> shifts = new List<Shift>();
        Random rand = new Random();

        // build roster
        foreach (var day in days)
        {
            foreach (var time in times)
            {
                var availableStaff = staffRoster.Staff
                    .Where(s =>
                        !s.AssignedShifts.Any(shift => shift.Day == day) &&
                        !s.UnavailableShifts.Any(u => u.Day == day && u.Time == time))
                    .OrderBy(x => rand.Next())
                    .Take(minStaffPerShift)
                    .ToList();

                var shift = new Shift { Day = day, Time = time, AssignedStaff = availableStaff };
                shifts.Add(shift);

                foreach (var staff in availableStaff)
                {
                    staff.AssignedShifts.Add(shift);
                }

                if (availableStaff.Count < minStaffPerShift)
                {
                    Console.WriteLine($"⚠️ Not enough staff for {day} {time} shift! ({availableStaff.Count}/{minStaffPerShift})");
                }
            }
        }

        ExportToExcel(shifts);
    }

    static void ExportToExcel(List<Shift> shifts)
    {
        // next Monday from now 
        DateTime today = DateTime.Today;
        int daysUntilMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
        DateTime nextMonday = today.AddDays(daysUntilMonday == 0 ? 7 : daysUntilMonday);
        string formattedDate = nextMonday.ToString("dd-MM-yy");

        // export loc
        string exportFolderPath = @"../../../examples/exports";
        if (!Directory.Exists(exportFolderPath))
        {
            Directory.CreateDirectory(exportFolderPath);
        }

        string fileName = Path.Combine(exportFolderPath, $"WC_{formattedDate}.xlsx");

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Shift Roster");

            // overarching title for the week
            worksheet.Cells[1, 1].Value = $"Week Commencing {nextMonday.ToString("dd MMM yyyy")}";
            worksheet.Cells[1, 1, 1, 3].Merge = true;  // Merge cells across the first row
            worksheet.Cells[1, 1].Style.Font.Size = 12;
            worksheet.Cells[1, 1].Style.Font.Bold = true;
            worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            worksheet.Cells[1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

            // heads
            worksheet.Cells[2, 1].Value = "Day";
            worksheet.Cells[2, 2].Value = "Time";
            worksheet.Cells[2, 3].Value = "Assigned Staff";

            int row = 3;  // start from row 3 since row 1 is the title and row 2 is for column headers
            foreach (var shift in shifts)
            {
                worksheet.Cells[row, 1].Value = shift.Day;
                worksheet.Cells[row, 2].Value = shift.Time;
                worksheet.Cells[row, 3].Value = string.Join(", ", shift.AssignedStaff.Select(s => s.Name));
                row++;
            }

            // Style headers
            using (var range = worksheet.Cells[2, 1, 2, 3])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }

            // Set column widths
            worksheet.Column(1).Width = 16; 
            worksheet.Column(2).Width = 16; 
            worksheet.Column(3).Width = 16; 

            // Save Excel file to the specified path
            File.WriteAllBytes(fileName, package.GetAsByteArray());
            Console.WriteLine($"\n✅ Excel file saved: {Path.GetFullPath(fileName)}");
        }
    }
}
