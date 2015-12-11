## Use Beam Width Files

Beam widths are currently provided in the form of C header files
that define beam widths with the `DEFINE_BEAMWIDTH3` macro. This macro
is not defined, so you can define it however you wish within your
project. One example might be something like the form below:

```
struct beam_info {
    double start, end;
};

#define DEFINE_BEAMWIDTH3(BEAMNUM,CENTER,START,END) { START, END },

const beam_info beam_widths_ARIS3000_128[] = {
#include "beam-width-metrics/BeamWidths_ARIS3000_128.h"
};

#undef DEFINE_BEAMWIDTH3
```
