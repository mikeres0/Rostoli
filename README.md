## Rostoli - C# Rostering Solution

Rostoli is a C# Rostering Solution using a "black box" style input/output system. It takes in a `staff_data.json` file, builds the roster and then exports it to the rosters export folder. The data for this application can be found at C:\Roster. Underneath that there are two folders (potentially one), one called config and one called exports. 

#### Config

The config folder contains `staff_data.json`. Within this file you can set out the shift days, patterns, workers and the days they're unable to work. When the program is run the software will interpret the data and generate the `.xlxs` file containing the roster information.

#### Exports
The exports folder simply contains the exported documents for the week commencing when the program was ran. So if it was ran on a Sunday (09/03/25) the file will be named commencing from the first Monday found, IE: 10/03/25. 
