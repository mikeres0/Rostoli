## Rostoli - C# Rostering Solution

Rostoli is a C# Rostering Solution using a "black box" style input/output system. It takes in a `staff_data.json` file, builds the roster and then exports it to the rosters export folder. The data for this application can be found at the project root in the Examples folder. Underneath that there are two folders one called config and one called exports. You'll find an example config file that can be used. You can modify the paths within the program should you wish to use different input files.

#### Config

The config folder contains `staff_data.json`. Within this file you can set out the shift days, patterns, workers and the days they're unable to work. When the program is run the software will interpret the data and generate the `.xlxs` file containing the roster information.

###### Root Object

- **config**: `object`
  - **days**: `array<string>`  
    List of days in the week (e.g., `"Monday"`, `"Tuesday"`, etc.)
  - **times**: `array<string>`  
    List of shift times (e.g., `"Morning"`, `"Midday"`, etc.)
  - **minStaffPerShift**: `integer`  
    Minimum number of staff required per shift

- **staff**: `array<object>`  
  List of staff members

  - Each staff member:
    - **name**: `string`  
      Unique name or ID of the staff member (e.g., `"Staff01"`)
    - **unavailableShifts**: `array<object>`  
      List of shifts the staff member is unavailable for (can be empty)

      - Each unavailable shift:
        - **day**: `string`  
          Day of the week (must match one of the `config.days`)
        - **time**: `string`  
          Time of day (must match one of the `config.times`)


#### Exports
The exports folder simply contains the exported documents for the week commencing when the program was ran. So if it was ran on a Sunday (09/03/25) the file will be named commencing from the first Monday found, IE: 10/03/25. 

##### Example Export
![image](https://github.com/user-attachments/assets/e7da78a7-3753-4f2e-8fa5-8bef7fd14c8f)

