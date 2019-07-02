# READ ME

## Generated Code Inside

This project uses a command script to build C# files in ./generated. These files
contain the beam with metrics.

**However,** there is a problem. The new dotnet project format appears to resolve
files before running the "pre-build" script, so the generated files are not part
of the build if they're not already on disk. So the project fails to build on first attempt.

A second attempt at building will succeed. The build server will run the script as a
precursor step to building this project.
