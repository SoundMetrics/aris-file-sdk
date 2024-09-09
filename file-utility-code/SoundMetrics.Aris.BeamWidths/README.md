# READ ME

## Generated Code Inside

This project parses the metrics files at runtime in order to build the beam widths tables.
The project must therefore include the metrics files as content and deliver it for deployment.

> Note: we have looked at using a source generator and it proved near-impossible to debug in the
VS 2022 timeframe. Therefore, we opted to parse at runtime. (This was all brought on by the change
in dotnet Core's way of determining build dependencies--if you generate code in a script in the pre-build
event or in a project on which you depend, those files don't exist at the time of dependency checking, so
it appears.)
