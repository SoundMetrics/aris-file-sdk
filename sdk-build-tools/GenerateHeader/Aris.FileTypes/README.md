## Aris.FileTypes

### Note on Paket Usage

This project uses Paket to fetch the BuildInfo.exe tool.

If BuildInfo.exe tools is updated, this project will need
to do the following in order to make use of the updated tool:

- Run `.\paket\paket.exe update` -- this will cause the dependencies
and lock file to be updated; these should then be committed.
