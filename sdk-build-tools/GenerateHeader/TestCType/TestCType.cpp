// TestCType.cpp : Defines the entry point for the console application.
//

#include <stdio.h>
#include <string.h>
#include <algorithm>

extern "C" {

#include "FileHeader.h"
#include "FrameHeader.h"

#pragma pack(push, 1)

    struct ArisFileHeaderFieldsOnly {
#include "FileHeaderFieldsOnly.h"
    };

    struct ArisFrameHeaderFieldsOnly {
#include "FrameHeaderFieldsOnly.h"
    };

#pragma pack(pop)

}

int main(int argc, char** argv)
{
    const auto ExpectedArgCount = 2;

    if (argc < ExpectedArgCount) {
        fprintf(stderr, "Too few arguments\n");
        return -1;
    }
    else if (argc > ExpectedArgCount) {
        fprintf(stderr, "Too many arguments\n");
        return -2;
    }
    else {
        auto typeName = argv[1];

        struct {
            const char* name;
            size_t size;
        } types[] = {
            { "ArisFileHeader",             sizeof ArisFileHeader },
            { "ArisFileHeaderFieldsOnly",   sizeof ArisFileHeaderFieldsOnly },
            { "ArisFrameHeader",            sizeof ArisFrameHeader },
            { "ArisFrameHeaderFieldsOnly",  sizeof ArisFrameHeaderFieldsOnly },
        };

        const auto it = std::find_if(std::begin(types), std::end(types),
            [typeName](const auto& ti) { return strcmp(typeName, ti.name) == 0; });

        if (it != std::end(types)) {
            printf("%s is %zu bytes\n", typeName, it->size);
        }
        else {
            fprintf(stderr, "Unexpected input: '%s'\n", typeName);
            return -3;
        }
    }

    return 0;
}
