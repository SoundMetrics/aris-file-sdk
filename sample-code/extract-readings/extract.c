// extract

#include "stdio.h"
#include "FileHeader.h"
#include "FrameHeader.h"
#include <string.h>

#define INVALID_INPUTS      -1
#define CANT_OPEN_INPUT     -2
#define CANT_OPEN_OUTPUT    -3
#define NOT_ARIS_FILE       -4
#define CORRUPT_ARIS_FILE   -5
#define IO_ERROR            -6

int validate_inputs(int argc,
                    char** argv,
                    const char** inputPath,
                    const char** outputPath);
void show_usage();
int extract(FILE* fpIn, FILE* fpOut);

int main(int argc, char** argv ) {

    const char* inputPath = NULL;
    const char* outputPath = NULL;
    FILE* fpIn = NULL;
    FILE* fpOut = NULL;

    if (validate_inputs(argc, argv, &inputPath, &outputPath)) {
        show_usage();
        return INVALID_INPUTS;
    }

    fpIn = fopen(inputPath, "r");
    if (!fpIn) {
        fprintf(stderr, "Couldn't open the input file.\n");
        return CANT_OPEN_INPUT;
    }

    fpOut = fopen(outputPath, "w");
    if (!fpOut) {
        fprintf(stderr, "Couldn't open output file.\n");
        fclose(fpIn);
        return CANT_OPEN_OUTPUT;
    }

    int result = extract(fpIn, fpOut);
    fclose(fpIn);
    fclose(fpOut);

    if (result) {
        fprintf(stderr, "An error occurred while extracting data.\n");
    }

    return result;
}

void show_usage() {

    fprintf(stderr, "USAGE:\n");
    fprintf(stderr, "    extract <input-path> <output-path>\n");
    fprintf(stderr, "\n");
}

int validate_inputs(int argc,
                    char** argv,
                    const char** inputPath,
                    const char** outputPath) {

    if (argc != 3) {
        fprintf(stderr, "Bad number of arguments.\n");
        return 1;
    }

    *inputPath = argv[1];
    *outputPath = argv[2];

    if (strlen(*inputPath) == 0) {
        fprintf(stderr, "No input path.\n");
        return 2;
    }

    if (strlen(*outputPath) == 0) {
        fprintf(stderr, "No output path.\n");
        return 3;
    }

    return 0;
}

#define ARIS_FILE_HEADER_SIZE 1024 // TODO generate a constant for size
#define ARIS_FRAME_HEADER_SIZE 1024 // TODO generate a constant for size

typedef union {
    char                    raw[ARIS_FILE_HEADER_SIZE]; // Force the correct size
    struct ArisFileHeader   header; // TODO make typedef in header
} FileHeaderBuffer;

typedef union {
    char                    raw[ARIS_FRAME_HEADER_SIZE]; // Force the correct size
    struct ArisFrameHeader  header;
} FrameHeaderBuffer;

#define FILE_SIGNATURE 0x05464444 // TODO generate a constant
#define FRAME_SIGNATURE 0x05464444 // TODO generate a constant

int extract(FILE* fpIn, FILE* fpOut) {

    FileHeaderBuffer fileHeaderBuf;
    FrameHeaderBuffer frameHeaderBuf;
    long fileSize, dataSize, frameSize, frameCount;

    if (fseek(fpIn, 0, SEEK_END)) {
        fprintf(stderr, "Couldn't determine file size.\n");
        return IO_ERROR;
    }

    fileSize = ftell(fpIn);
    fseek(fpIn, 0, SEEK_SET);
    dataSize = fileSize - ARIS_FILE_HEADER_SIZE;

    if (fread(&fileHeaderBuf, sizeof(fileHeaderBuf), 1, fpIn) != 1) {
        fprintf(stderr, "Couldn't read complete file header.\n");
        return NOT_ARIS_FILE;
    }

    if (fileHeaderBuf.header.F1 != FILE_SIGNATURE) {
        fprintf(stderr, "Invalid file header.\n");
        return NOT_ARIS_FILE;
    }

    if (fread(&frameHeaderBuf, sizeof(frameHeaderBuf), 1, fpIn) != 1) {
        fprintf(stderr, "Couldn't read first frame buffer.\n");
        return CORRUPT_ARIS_FILE;
    }

    frameSize = frameHeaderBuf.header.F1 * frameHeaderBuf.header.F1;
    frameCount = dataSize / frameSize;

    fprintf(fpOut, "FrameIndex,FrameTime,Temp,Depth\n");

    do {
        // Dump frame info
        // TODO

        // Skip over the frame data
        fseek(fpIn, frameSize, SEEK_CUR);

    } while (fread(&frameHeaderBuf, sizeof(frameHeaderBuf), 1, fpIn) == 1);

    return 0;
}
