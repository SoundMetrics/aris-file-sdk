// FrameHeader.h

#ifndef ARIS_ARISFRAMEHEADER_H
#define ARIS_ARISFRAMEHEADER_H

#include <stdint.h>

#pragma pack(push, 1)

struct ArisFrameHeader {

    // Frame number in file
    uint32_t FrameIndex;

    // PC time stamp when recorded; microseconds since epoch (Jan 1st 1970)
    uint64_t FrameTime;

    // ARIS file format version = 0x05464444
    uint32_t Version;

    uint32_t Status;

    // On-sonar microseconds since epoch (Jan 1st 1970)
    uint64_t sonarTimeStamp;

    uint32_t TS_Day;

    uint32_t TS_Hour;

    uint32_t TS_Minute;

    uint32_t TS_Second;

    uint32_t TS_Hsecond;

    uint32_t TransmitMode;

    // Window start in meters
    float WindowStart;

    // Window length in meters
    float WindowLength;

    uint32_t Threshold;

    int32_t Intensity;

    // In frame  fromsonar?
    uint32_t ReceiverGain;

    // In frame  fromsonar? Which values?
    uint32_t DegC1;

    // In frame  fromsonar? Which values?
    uint32_t DegC2;

    // review
    uint32_t Humidity;

    // review
    uint32_t Focus;

    // review
    uint32_t Battery;

    // port 700 user data?
    float UserValue1;

    float UserValue2;

    float UserValue3;

    float UserValue4;

    float UserValue5;

    float UserValue6;

    float UserValue7;

    float UserValue8;

    // Platform velocity from AUV integration
    float Velocity;

    // Platform depth from AUV integration
    float Depth;

    // Platform altitude from AUV integration
    float Altitude;

    // Platform pitch from AUV integration
    float Pitch;

    // Platform pitch rate from AUV integration
    float PitchRate;

    // Platform roll from AUV integration
    float Roll;

    // Platform roll rate from AUV integration
    float RollRate;

    // Platform heading from AUV integration
    float Heading;

    // Platform heading rate from AUV integration
    float HeadingRate;

    // Sonar compass heading output
    float CompassHeading;

    // Sonar compass pitch output
    float CompassPitch;

    // Sonar compass roll output
    float CompassRoll;

    // from auxiliary GPS sensor
    double Latitude;

    // from auxiliary GPS sensor
    double Longitude;

    // Note: special for PNNL
    float SonarPosition;

    uint32_t ConfigFlags;

    float BeamTilt;

    float TargetRange;

    float TargetBearing;

    uint32_t TargetPresent;

    // review
    uint32_t FirmwareRevision;

    uint32_t Flags;

    // Source file frame number for CSOT output files
    uint32_t SourceFrame;

    // Water temperature from housing temperature sensor
    float WaterTemp;

    uint32_t TimerPeriod;

    // Sonar X location for 3D processing
    // Note: Bluefin, external sensor data
    float SonarX;

    // Sonar Y location for 3D processing
    float SonarY;

    // Sonar Z location for 3D processing
    float SonarZ;

    // X2 pan output
    float SonarPan;

    // X2 tilt output
    float SonarTilt;

    // X2 roll output
    float SonarRoll;

    float PanPNNL;

    float TiltPNNL;

    float RollPNNL;

    // Note: special for Bluefin HAUV or other AUV integration
    double VehicleTime;

    // GPS output from NMEA GGK message
    float TimeGGK;

    // GPS output from NMEA GGK message
    uint32_t DateGGK;

    // GPS output from NMEA GGK message
    uint32_t QualityGGK;

    // GPS output from NMEA GGK message
    uint32_t NumSatsGGK;

    // GPS output from NMEA GGK message
    float DOPGGK;

    // GPS output from NMEA GGK message
    float EHTGGK;

    // external sensor
    float HeaveTSS;

    // GPS year output
    uint32_t YearGPS;

    // GPS month output
    uint32_t MonthGPS;

    // GPS day output
    uint32_t DayGPS;

    // GPS hour output
    uint32_t HourGPS;

    // GPS minute output
    uint32_t MinuteGPS;

    // GPS second output
    uint32_t SecondGPS;

    // GPS 1/100th second output
    uint32_t HSecondGPS;

    // Sonar mount location pan offset for 3D processing; meters
    float SonarPanOffset;

    // Sonar mount location tilt offset for 3D processing
    float SonarTiltOffset;

    // Sonar mount location roll offset for 3D processing
    float SonarRollOffset;

    // Sonar mount location X offset for 3D processing
    float SonarXOffset;

    // Sonar mount location Y offset for 3D processing
    float SonarYOffset;

    // Sonar mount location Z offset for 3D processing
    float SonarZOffset;

    // 3D processing transformation matrix
    float Tmatrix[16];

    // Calculated as 1e6/SamplePeriod
    float SampleRate;

    // X-axis sonar acceleration
    float AccellX;

    // Y-axis sonar acceleration
    float AccellY;

    // Z-axis sonar acceleration
    float AccellZ;

    // ARIS ping mode
    // Note: 1..12
    uint32_t PingMode;

    // Frequency
    // Note: 1 = HF, 0 = LF
    uint32_t FrequencyHiLow;

    // Width of transmit pulse
    // Note: 4..100 microseconds
    uint32_t PulseWidth;

    // Ping cycle time
    // Note: 1802..65535 microseconds
    uint32_t CyclePeriod;

    // Downrange sample rate
    // Note: 4..100 microseconds
    uint32_t SamplePeriod;

    // 1 = Transmit ON, 0 = Transmit OFF
    uint32_t TransmitEnable;

    // Instantaneous frame rate between frame N and frame N-1
    // Note: microseconds
    float FrameRate;

    // Sound velocity in water calculated from water temperature and salinity setting
    // Note: m/s
    float SoundSpeed;

    // Number of downrange samples in each beam
    uint32_t SamplesPerBeam;

    // 1 = 150V ON (Max Power), 0 = 150V OFF (Min Power, 12V)
    uint32_t Enable150V;

    // Delay from transmit until start of sampling (window start) in usec, [930..65535]
    uint32_t SampleStartDelay;

    // 1 = telephoto lens (large lens, big lens, hi-res lens) present
    uint32_t LargeLens;

    // 1 = ARIS 3000, 0 = ARIS 1800, 2 = ARIS 1200
    uint32_t TheSystemType;

    // Sonar serial number as labeled on housing
    uint32_t SonarSerialNumber;

    // Reserved.
    // OBSOLETE: Obsolete
    uint64_t ReservedEK;

    // review: Error flag code bits
    uint32_t ArisErrorFlagsUint;

    // Missed packet count for Ethernet statistics reporting
    uint32_t MissedPackets;

    // Version number of ArisApp sending frame data
    uint32_t ArisAppVersion;

    // Reserved for future use
    uint32_t Available2;

    // 1 = frame data already ordered into [beam,sample] array, 0 = needs reordering
    uint32_t ReorderedSamples;

    // Water salinity code:  0 = fresh, 15 = brackish, 35 = salt
    uint32_t Salinity;

    // Depth sensor output in meters
    float Pressure;

    // Battery input voltage before power steering
    float BatteryVoltage;

    // Main cable input voltage before power steering
    float MainVoltage;

    // Input voltage after power steering; filtered voltage now
    float SwitchVoltage;

    // Note: Added 14-Aug-2012 for AutomaticRecording
    uint32_t FocusMotorMoving;

    // Note: Added 16-Aug (first two bits = 12V, second two bits = 150V, 00 = not changing, 01 = turning on, 10 = turning off)
    uint32_t VoltageChanging;

    uint32_t FocusTimeoutFault;

    uint32_t FocusOverCurrentFault;

    uint32_t FocusNotFoundFault;

    uint32_t FocusStalledFault;

    uint32_t FPGATimeoutFault;

    uint32_t FPGABusyFault;

    uint32_t FPGAStuckFault;

    uint32_t CPUTempFault;

    uint32_t PSUTempFault;

    uint32_t WaterTempFault;

    uint32_t HumidityFault;

    uint32_t PressureFault;

    uint32_t VoltageReadFault;

    uint32_t VoltageWriteFault;

    // Focus shaft current position
    // Note: 0..1000 motor units
    uint32_t FocusCurrentPosition;

    // Commanded pan position
    float TargetPan;

    // Commanded tilt position
    float TargetTilt;

    // Commanded roll position
    float TargetRoll;

    uint32_t PanMotorErrorCode;

    uint32_t TiltMotorErrorCode;

    uint32_t RollMotorErrorCode;

    // Low-resolution magnetic encoder absolute pan position
    float PanAbsPosition;

    // Low-resolution magnetic encoder absolute tilt position
    float TiltAbsPosition;

    // Low-resolution magnetic encoder absolute roll position
    float RollAbsPosition;

    // Accelerometer outputs from AR2 CPU board sensor
    float PanAccelX;

    float PanAccelY;

    float PanAccelZ;

    float TiltAccelX;

    float TiltAccelY;

    float TiltAccelZ;

    float RollAccelX;

    float RollAccelY;

    float RollAccelZ;

    // Cookie indices for command acknowlege in frame header
    uint32_t AppliedSettings;

    // Cookie indices for command acknowlege in frame header
    uint32_t ConstrainedSettings;

    // Cookie indices for command acknowlege in frame header
    uint32_t InvalidSettings;

    // If true delay is added between sending out image data packets
    uint32_t EnableInterpacketDelay;

    // packet delay factor in us (does not include function overhead time)
    uint32_t InterpacketDelayPeriod;

    // Total number of seconds sonar has been running; session or lifetime?
    uint32_t Uptime;

    // Major version number
    uint16_t ArisAppVersionMajor;

    // Minor version number
    uint16_t ArisAppVersionMinor;

    // Sonar time when frame cycle is initiated in hardware
    uint64_t GoTime;

    // AR2 pan velocity
    // Note: degrees/second
    float PanVelocity;

    // AR2 tilt velocity
    // Note: degrees/second
    float TiltVelocity;

    // AR2 roll velocity
    // Note: degrees/second
    float RollVelocity;

    // Age of the last GPS fix acquired; capped at 0xFFFFFFFF; zero if none
    // Note: microseconds
    uint32_t GpsTimeAge;

    // Padding to fill out to 1024 bytes
    char padding[292];

};

#pragma pack(pop)

#endif // !ARIS_ARISFRAMEHEADER_H
