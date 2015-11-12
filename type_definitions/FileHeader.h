// FileHeader.h

#ifndef ARIS_ARISFILEHEADER_H
#define ARIS_ARISFILEHEADER_H

#include <stdint.h>

#pragma pack(push, 1)

struct ArisFileHeader {

    // Description of this field. 12345678901234567890
    int32_t F1;

    // My F2
    // Note: F2's notes and ramblings go here!!!1!
    // OBSOLETE: As of 1/1/2016 use F3 instead.
    char Field2[32];

    // F3!
    char F3[32];

};

#pragma pack(pop)

#endif // !ARIS_ARISFILEHEADER_H
