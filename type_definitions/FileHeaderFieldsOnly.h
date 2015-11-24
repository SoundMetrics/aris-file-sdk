    // File format version DDF_05 = 0x05464444
    uint32_t Version;

    // Total frames in file
    // OBSOLETE: Calculate the number of frames from file size & beams*samples.
    uint32_t FrameCount;

    // Initial recorded frame rate
    // OBSOLETE: See frame header instead.
    uint32_t FrameRate;

    // Non-zero if HF, zero if LF
    // OBSOLETE: See frame header instead.
    uint32_t HighResolution;

    // ARIS 3000 = 128/64, ARIS 1800 = 96/48, ARIS 1200 = 48
    // OBSOLETE: See frame header instead.
    uint32_t NumRawBeams;

    // 1/Sample Period
    // OBSOLETE: See frame header instead.
    float SampleRate;

    // Number of range samples in each beam
    // OBSOLETE: See frame header instead.
    uint32_t SamplesPerChannel;

    // Relative gain in dB:  0 - 40
    // OBSOLETE: See frame header instead.
    uint32_t ReceiverGain;

    // Image window start range in meters (code [0..31] in DIDSON)
    // OBSOLETE: See frame header instead.
    float WindowStart;

    // Image window length in meters  (code [0..3] in DIDSON)
    // OBSOLETE: See frame header instead.
    float WindowLength;

    // Non-zero = lens down (DIDSON) or lens up (ARIS), zero = opposite
    // OBSOLETE: See frame header instead.
    uint32_t Reverse;

    // Sonar serial number
    uint32_t SN;

    // Date that file was recorded
    char strDate[32];

    // User input to identify file in 256 characters
    char strHeaderID[256];

    // User-defined integer quantity
    int32_t UserID1;

    // User-defined integer quantity
    int32_t UserID2;

    // User-defined integer quantity
    int32_t UserID3;

    // User-defined integer quantity
    int32_t UserID4;

    // First frame number from source file (for DIDSON snippet files)
    uint32_t StartFrame;

    // Last frame number from source file (for DIDSON snippet files)
    uint32_t EndFrame;

    // Non-zero indicates time lapse recording
    uint32_t TimeLapse;

    // Number of frames/seconds between recorded frames
    uint32_t RecordInterval;

    // Frames or seconds interval
    uint32_t RadioSeconds;

    // Record every Nth frame
    uint32_t FrameInterval;

    // See DDF_04 file format document
    // OBSOLETE: Obsolete.
    uint32_t Flags;

    // See DDF_04 file format document
    uint32_t AuxFlags;

    // Sound velocity in water
    // OBSOLETE: See frame header instead.
    uint32_t Sspd;

    // See DDF_04 file format document
    uint32_t Flags3D;

    // DIDSON software version that recorded the file
    uint32_t SoftwareVersion;

    // Water temperature code:  0 = 5-15C, 1 = 15-25C, 2 = 25-35C
    uint32_t WaterTemp;

    // Salinity code:  0 = fresh, 1 = brackish, 2 = salt
    uint32_t Salinity;

    // Added for ARIS but not used
    uint32_t PulseLength;

    // Added for ARIS but not used
    uint32_t TxMode;

    // Reserved for future use
    uint32_t VersionFGPA;

    // Reserved for future use
    uint32_t VersionPSuC;

    // Frame index of frame used for thumbnail image of file
    uint32_t ThumbnailFI;

    // Total file size in bytes
    // OBSOLETE: Do not use; query your filesystem instead.
    uint64_t FileSize;

    // Reserved for future use
    // OBSOLETE: Obsolete; not used.
    uint64_t OptionalHeaderSize;

    // Reserved for future use
    // OBSOLETE: Obsolete; not used.
    uint64_t OptionalTailSize;

    // DIDSON_ADJUSTED_VERSION_MINOR
    // OBSOLETE: Obsolete.
    uint32_t VersionMinor;

    // Non-zero if telephoto lens (large lens, hi-res lens, big lens) is present
    // OBSOLETE: See frame header instead.
    uint32_t LargeLens;

    // Padding to fill out to 1024 bytes
    char padding[568];

