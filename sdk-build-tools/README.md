## Build Instructions

### Audience

These instructions are largely for internal Sound Metrics use.

We encourage you to get the headers and bits from
[Releases](https://github.com/SoundMetrics/aris-file-sdk/releases).

### Dependencies

The following are the current dependencies for building:

- Visual Studio 2013: C++
- Visual Studio 2015: C++, C#, F#

### Build Steps

In a Visual Studio native x86 tools command prompt
- run build.cmd. This builds the bits and sample bits.
- run build-packages.cmd. This requires a command line argument in the
form w.x.y.z to pass the version being built when building the Nuget packages.
These packages are created in .\NugetPkg.

### Marking a Release

Be sure to add the newly built Nuget packages to the release.
