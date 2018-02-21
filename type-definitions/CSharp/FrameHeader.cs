// FrameHeader.cs

// THIS IS GENERATED WITH GenerateHeader, DO NOT MODIFY

namespace Aris.FileTypes
{
    
    using System;
    using System.Runtime.InteropServices;

    // Defines the metadata at the start of an ARIS frame.
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct ArisFrameHeader
    {
        public const uint ArisFileSignature =  0x05464444;
        public const uint ArisFrameSignature = 0x05464444;

        // Frame number in file
        public UInt32 FrameIndex;

        // PC time stamp when recorded; microseconds since epoch (Jan 1st 1970)
        public UInt64 FrameTime;

        // ARIS file format version = 0x05464444
        public UInt32 Version;

        public UInt32 Status;

        // On-sonar microseconds since epoch (Jan 1st 1970)
        public UInt64 sonarTimeStamp;

        public UInt32 TS_Day;

        public UInt32 TS_Hour;

        public UInt32 TS_Minute;

        public UInt32 TS_Second;

        public UInt32 TS_Hsecond;

        public UInt32 TransmitMode;

        // Window start in meters
        public float WindowStart;

        // Window length in meters
        public float WindowLength;

        public UInt32 Threshold;

        public int Intensity;

        // Note: 0-24 dB
        public UInt32 ReceiverGain;

        // CPU temperature
        // Note: Celsius
        public UInt32 DegC1;

        // Power supply temperature
        // Note: Celsius
        public UInt32 DegC2;

        // % relative humidity
        public UInt32 Humidity;

        // Focus units 0-1000
        public UInt32 Focus;

        [Obsolete("Unused.")]
        public UInt32 Battery;

        public float UserValue1;

        public float UserValue2;

        public float UserValue3;

        public float UserValue4;

        public float UserValue5;

        public float UserValue6;

        public float UserValue7;

        public float UserValue8;

        // Platform velocity from AUV integration
        public float Velocity;

        // Platform depth from AUV integration
        public float Depth;

        // Platform altitude from AUV integration
        public float Altitude;

        // Platform pitch from AUV integration
        public float Pitch;

        // Platform pitch rate from AUV integration
        public float PitchRate;

        // Platform roll from AUV integration
        public float Roll;

        // Platform roll rate from AUV integration
        public float RollRate;

        // Platform heading from AUV integration
        public float Heading;

        // Platform heading rate from AUV integration
        public float HeadingRate;

        // Sonar compass heading output
        public float CompassHeading;

        // Sonar compass pitch output
        public float CompassPitch;

        // Sonar compass roll output
        public float CompassRoll;

        // from auxiliary GPS sensor
        public double Latitude;

        // from auxiliary GPS sensor
        public double Longitude;

        // Note: special for PNNL
        public float SonarPosition;

        public UInt32 ConfigFlags;

        public float BeamTilt;

        public float TargetRange;

        public float TargetBearing;

        public UInt32 TargetPresent;

        [Obsolete("Unused.")]
        public UInt32 FirmwareRevision;

        public UInt32 Flags;

        // Source file frame number for CSOT output files
        public UInt32 SourceFrame;

        // Water temperature from housing temperature sensor
        public float WaterTemp;

        public UInt32 TimerPeriod;

        // Sonar X location for 3D processing
        // Note: Bluefin, external sensor data
        public float SonarX;

        // Sonar Y location for 3D processing
        public float SonarY;

        // Sonar Z location for 3D processing
        public float SonarZ;

        // X2 pan output
        public float SonarPan;

        // X2 tilt output
        public float SonarTilt;

        // X2 roll output
        public float SonarRoll;

        public float PanPNNL;

        public float TiltPNNL;

        public float RollPNNL;

        // Note: special for Bluefin HAUV or other AUV integration
        public double VehicleTime;

        // GPS output from NMEA GGK message
        public float TimeGGK;

        // GPS output from NMEA GGK message
        public UInt32 DateGGK;

        // GPS output from NMEA GGK message
        public UInt32 QualityGGK;

        // GPS output from NMEA GGK message
        public UInt32 NumSatsGGK;

        // GPS output from NMEA GGK message
        public float DOPGGK;

        // GPS output from NMEA GGK message
        public float EHTGGK;

        // external sensor
        public float HeaveTSS;

        // GPS year output
        public UInt32 YearGPS;

        // GPS month output
        public UInt32 MonthGPS;

        // GPS day output
        public UInt32 DayGPS;

        // GPS hour output
        public UInt32 HourGPS;

        // GPS minute output
        public UInt32 MinuteGPS;

        // GPS second output
        public UInt32 SecondGPS;

        // GPS 1/100th second output
        public UInt32 HSecondGPS;

        // Sonar mount location pan offset for 3D processing; meters
        public float SonarPanOffset;

        // Sonar mount location tilt offset for 3D processing
        public float SonarTiltOffset;

        // Sonar mount location roll offset for 3D processing
        public float SonarRollOffset;

        // Sonar mount location X offset for 3D processing
        public float SonarXOffset;

        // Sonar mount location Y offset for 3D processing
        public float SonarYOffset;

        // Sonar mount location Z offset for 3D processing
        public float SonarZOffset;

        // 3D processing transformation matrix
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public float[] Tmatrix;

        // Calculated as 1e6/SamplePeriod
        public float SampleRate;

        // X-axis sonar acceleration
        public float AccellX;

        // Y-axis sonar acceleration
        public float AccellY;

        // Z-axis sonar acceleration
        public float AccellZ;

        // ARIS ping mode
        // Note: 1..12
        public UInt32 PingMode;

        // Frequency
        // Note: 1 = HF, 0 = LF
        public UInt32 FrequencyHiLow;

        // Width of transmit pulse
        // Note: 4..100 microseconds
        public UInt32 PulseWidth;

        // Ping cycle time
        // Note: 1802..65535 microseconds
        public UInt32 CyclePeriod;

        // Downrange sample rate
        // Note: 4..100 microseconds
        public UInt32 SamplePeriod;

        // 1 = Transmit ON, 0 = Transmit OFF
        public UInt32 TransmitEnable;

        // Instantaneous frame rate between frame N and frame N-1
        // Note: microseconds
        public float FrameRate;

        // Sound velocity in water calculated from water temperature depth and salinity setting
        // Note: m/s
        public float SoundSpeed;

        // Number of downrange samples in each beam
        public UInt32 SamplesPerBeam;

        // 1 = 150V ON (Max Power), 0 = 150V OFF (Min Power, 12V)
        public UInt32 Enable150V;

        // Delay from transmit until start of sampling (window start) in usec, [930..65535]
        public UInt32 SampleStartDelay;

        // 1 = telephoto lens (large lens, big lens, hi-res lens) present
        public UInt32 LargeLens;

        // 1 = ARIS 3000, 0 = ARIS 1800, 2 = ARIS 1200
        public UInt32 TheSystemType;

        // Sonar serial number as labeled on housing
        public UInt32 SonarSerialNumber;

        // Reserved.
        [Obsolete("Obsolete")]
        public UInt64 ReservedEK;

        // Error flag code bits
        public UInt32 ArisErrorFlagsUint;

        // Missed packet count for Ethernet statistics reporting
        public UInt32 MissedPackets;

        // Version number of ArisApp sending frame data
        public UInt32 ArisAppVersion;

        // Reserved for future use
        public UInt32 Available2;

        // 1 = frame data already ordered into [beam,sample] array, 0 = needs reordering
        public UInt32 ReorderedSamples;

        // Water salinity code:  0 = fresh, 15 = brackish, 35 = salt
        public UInt32 Salinity;

        // Depth sensor output
        // Note: psi
        public float Pressure;

        // Battery input voltage before power steering
        // Note: mV
        public float BatteryVoltage;

        // Main cable input voltage before power steering
        // Note: mV
        public float MainVoltage;

        // Input voltage after power steering; filtered voltage
        // Note: mV
        public float SwitchVoltage;

        // Note: Added 14-Aug-2012 for AutomaticRecording
        public UInt32 FocusMotorMoving;

        // Note: Added 16-Aug (first two bits = 12V, second two bits = 150V, 00 = not changing, 01 = turning on, 10 = turning off)
        public UInt32 VoltageChanging;

        public UInt32 FocusTimeoutFault;

        public UInt32 FocusOverCurrentFault;

        public UInt32 FocusNotFoundFault;

        public UInt32 FocusStalledFault;

        public UInt32 FPGATimeoutFault;

        public UInt32 FPGABusyFault;

        public UInt32 FPGAStuckFault;

        public UInt32 CPUTempFault;

        public UInt32 PSUTempFault;

        public UInt32 WaterTempFault;

        public UInt32 HumidityFault;

        public UInt32 PressureFault;

        public UInt32 VoltageReadFault;

        public UInt32 VoltageWriteFault;

        // Focus shaft current position
        // Note: 0..1000 motor units
        public UInt32 FocusCurrentPosition;

        // Commanded pan position
        public float TargetPan;

        // Commanded tilt position
        public float TargetTilt;

        // Commanded roll position
        public float TargetRoll;

        public UInt32 PanMotorErrorCode;

        public UInt32 TiltMotorErrorCode;

        public UInt32 RollMotorErrorCode;

        // Low-resolution magnetic encoder absolute pan position (NaN indicates no arm detected for axis since 2.6.0.8403)
        public float PanAbsPosition;

        // Low-resolution magnetic encoder absolute tilt position (NaN indicates no arm detected for axis since 2.6.0.8403)
        public float TiltAbsPosition;

        // Low-resolution magnetic encoder absolute roll position (NaN indicates no arm detected for axis since 2.6.0.8403)
        public float RollAbsPosition;

        // Accelerometer outputs from AR2 CPU board sensor
        // Note: G
        public float PanAccelX;

        // Note: G
        public float PanAccelY;

        // Note: G
        public float PanAccelZ;

        // Note: G
        public float TiltAccelX;

        // Note: G
        public float TiltAccelY;

        // Note: G
        public float TiltAccelZ;

        // Note: G
        public float RollAccelX;

        // Note: G
        public float RollAccelY;

        // Note: G
        public float RollAccelZ;

        // Cookie indices for command acknowlege in frame header
        public UInt32 AppliedSettings;

        // Cookie indices for command acknowlege in frame header
        public UInt32 ConstrainedSettings;

        // Cookie indices for command acknowlege in frame header
        public UInt32 InvalidSettings;

        // If true delay is added between sending out image data packets
        public UInt32 EnableInterpacketDelay;

        // packet delay factor in us (does not include function overhead time)
        public UInt32 InterpacketDelayPeriod;

        // Total time the sonar has been running over its lifetime.
        // Note: seconds
        public UInt32 Uptime;

        // Major version number
        public UInt16 ArisAppVersionMajor;

        // Minor version number
        public UInt16 ArisAppVersionMinor;

        // Sonar time when frame cycle is initiated in hardware
        public UInt64 GoTime;

        // AR2 pan velocity
        // Note: degrees/second
        public float PanVelocity;

        // AR2 tilt velocity
        // Note: degrees/second
        public float TiltVelocity;

        // AR2 roll velocity
        // Note: degrees/second
        public float RollVelocity;

        // Age of the last GPS fix acquired; capped at 0xFFFFFFFF; zero if none
        // Note: microseconds
        public UInt32 GpsTimeAge;

        // bit 0 = Defender
        public UInt32 SystemVariant;

        // Padding to fill out to 1024 bytes
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 288)]
        public string padding;

    }

    public static class ArisFrameHeaderOffsets
    {
        public static UInt32 FrameIndex                     =    0;

        public static UInt32 FrameTime                      =    4;

        public static UInt32 Version                        =   12;

        public static UInt32 Status                         =   16;

        public static UInt32 sonarTimeStamp                 =   20;

        public static UInt32 TS_Day                         =   28;

        public static UInt32 TS_Hour                        =   32;

        public static UInt32 TS_Minute                      =   36;

        public static UInt32 TS_Second                      =   40;

        public static UInt32 TS_Hsecond                     =   44;

        public static UInt32 TransmitMode                   =   48;

        public static UInt32 WindowStart                    =   52;

        public static UInt32 WindowLength                   =   56;

        public static UInt32 Threshold                      =   60;

        public static UInt32 Intensity                      =   64;

        public static UInt32 ReceiverGain                   =   68;

        public static UInt32 DegC1                          =   72;

        public static UInt32 DegC2                          =   76;

        public static UInt32 Humidity                       =   80;

        public static UInt32 Focus                          =   84;

        [Obsolete("Unused.")]
        public static UInt32 Battery                        =   88;

        public static UInt32 UserValue1                     =   92;

        public static UInt32 UserValue2                     =   96;

        public static UInt32 UserValue3                     =  100;

        public static UInt32 UserValue4                     =  104;

        public static UInt32 UserValue5                     =  108;

        public static UInt32 UserValue6                     =  112;

        public static UInt32 UserValue7                     =  116;

        public static UInt32 UserValue8                     =  120;

        public static UInt32 Velocity                       =  124;

        public static UInt32 Depth                          =  128;

        public static UInt32 Altitude                       =  132;

        public static UInt32 Pitch                          =  136;

        public static UInt32 PitchRate                      =  140;

        public static UInt32 Roll                           =  144;

        public static UInt32 RollRate                       =  148;

        public static UInt32 Heading                        =  152;

        public static UInt32 HeadingRate                    =  156;

        public static UInt32 CompassHeading                 =  160;

        public static UInt32 CompassPitch                   =  164;

        public static UInt32 CompassRoll                    =  168;

        public static UInt32 Latitude                       =  172;

        public static UInt32 Longitude                      =  180;

        public static UInt32 SonarPosition                  =  188;

        public static UInt32 ConfigFlags                    =  192;

        public static UInt32 BeamTilt                       =  196;

        public static UInt32 TargetRange                    =  200;

        public static UInt32 TargetBearing                  =  204;

        public static UInt32 TargetPresent                  =  208;

        [Obsolete("Unused.")]
        public static UInt32 FirmwareRevision               =  212;

        public static UInt32 Flags                          =  216;

        public static UInt32 SourceFrame                    =  220;

        public static UInt32 WaterTemp                      =  224;

        public static UInt32 TimerPeriod                    =  228;

        public static UInt32 SonarX                         =  232;

        public static UInt32 SonarY                         =  236;

        public static UInt32 SonarZ                         =  240;

        public static UInt32 SonarPan                       =  244;

        public static UInt32 SonarTilt                      =  248;

        public static UInt32 SonarRoll                      =  252;

        public static UInt32 PanPNNL                        =  256;

        public static UInt32 TiltPNNL                       =  260;

        public static UInt32 RollPNNL                       =  264;

        public static UInt32 VehicleTime                    =  268;

        public static UInt32 TimeGGK                        =  276;

        public static UInt32 DateGGK                        =  280;

        public static UInt32 QualityGGK                     =  284;

        public static UInt32 NumSatsGGK                     =  288;

        public static UInt32 DOPGGK                         =  292;

        public static UInt32 EHTGGK                         =  296;

        public static UInt32 HeaveTSS                       =  300;

        public static UInt32 YearGPS                        =  304;

        public static UInt32 MonthGPS                       =  308;

        public static UInt32 DayGPS                         =  312;

        public static UInt32 HourGPS                        =  316;

        public static UInt32 MinuteGPS                      =  320;

        public static UInt32 SecondGPS                      =  324;

        public static UInt32 HSecondGPS                     =  328;

        public static UInt32 SonarPanOffset                 =  332;

        public static UInt32 SonarTiltOffset                =  336;

        public static UInt32 SonarRollOffset                =  340;

        public static UInt32 SonarXOffset                   =  344;

        public static UInt32 SonarYOffset                   =  348;

        public static UInt32 SonarZOffset                   =  352;

        public static UInt32 Tmatrix                        =  356;

        public static UInt32 SampleRate                     =  420;

        public static UInt32 AccellX                        =  424;

        public static UInt32 AccellY                        =  428;

        public static UInt32 AccellZ                        =  432;

        public static UInt32 PingMode                       =  436;

        public static UInt32 FrequencyHiLow                 =  440;

        public static UInt32 PulseWidth                     =  444;

        public static UInt32 CyclePeriod                    =  448;

        public static UInt32 SamplePeriod                   =  452;

        public static UInt32 TransmitEnable                 =  456;

        public static UInt32 FrameRate                      =  460;

        public static UInt32 SoundSpeed                     =  464;

        public static UInt32 SamplesPerBeam                 =  468;

        public static UInt32 Enable150V                     =  472;

        public static UInt32 SampleStartDelay               =  476;

        public static UInt32 LargeLens                      =  480;

        public static UInt32 TheSystemType                  =  484;

        public static UInt32 SonarSerialNumber              =  488;

        [Obsolete("Obsolete")]
        public static UInt32 ReservedEK                     =  492;

        public static UInt32 ArisErrorFlagsUint             =  500;

        public static UInt32 MissedPackets                  =  504;

        public static UInt32 ArisAppVersion                 =  508;

        public static UInt32 Available2                     =  512;

        public static UInt32 ReorderedSamples               =  516;

        public static UInt32 Salinity                       =  520;

        public static UInt32 Pressure                       =  524;

        public static UInt32 BatteryVoltage                 =  528;

        public static UInt32 MainVoltage                    =  532;

        public static UInt32 SwitchVoltage                  =  536;

        public static UInt32 FocusMotorMoving               =  540;

        public static UInt32 VoltageChanging                =  544;

        public static UInt32 FocusTimeoutFault              =  548;

        public static UInt32 FocusOverCurrentFault          =  552;

        public static UInt32 FocusNotFoundFault             =  556;

        public static UInt32 FocusStalledFault              =  560;

        public static UInt32 FPGATimeoutFault               =  564;

        public static UInt32 FPGABusyFault                  =  568;

        public static UInt32 FPGAStuckFault                 =  572;

        public static UInt32 CPUTempFault                   =  576;

        public static UInt32 PSUTempFault                   =  580;

        public static UInt32 WaterTempFault                 =  584;

        public static UInt32 HumidityFault                  =  588;

        public static UInt32 PressureFault                  =  592;

        public static UInt32 VoltageReadFault               =  596;

        public static UInt32 VoltageWriteFault              =  600;

        public static UInt32 FocusCurrentPosition           =  604;

        public static UInt32 TargetPan                      =  608;

        public static UInt32 TargetTilt                     =  612;

        public static UInt32 TargetRoll                     =  616;

        public static UInt32 PanMotorErrorCode              =  620;

        public static UInt32 TiltMotorErrorCode             =  624;

        public static UInt32 RollMotorErrorCode             =  628;

        public static UInt32 PanAbsPosition                 =  632;

        public static UInt32 TiltAbsPosition                =  636;

        public static UInt32 RollAbsPosition                =  640;

        public static UInt32 PanAccelX                      =  644;

        public static UInt32 PanAccelY                      =  648;

        public static UInt32 PanAccelZ                      =  652;

        public static UInt32 TiltAccelX                     =  656;

        public static UInt32 TiltAccelY                     =  660;

        public static UInt32 TiltAccelZ                     =  664;

        public static UInt32 RollAccelX                     =  668;

        public static UInt32 RollAccelY                     =  672;

        public static UInt32 RollAccelZ                     =  676;

        public static UInt32 AppliedSettings                =  680;

        public static UInt32 ConstrainedSettings            =  684;

        public static UInt32 InvalidSettings                =  688;

        public static UInt32 EnableInterpacketDelay         =  692;

        public static UInt32 InterpacketDelayPeriod         =  696;

        public static UInt32 Uptime                         =  700;

        public static UInt32 ArisAppVersionMajor            =  704;

        public static UInt32 ArisAppVersionMinor            =  706;

        public static UInt32 GoTime                         =  708;

        public static UInt32 PanVelocity                    =  716;

        public static UInt32 TiltVelocity                   =  720;

        public static UInt32 RollVelocity                   =  724;

        public static UInt32 GpsTimeAge                     =  728;

        public static UInt32 SystemVariant                  =  732;

    }

}
