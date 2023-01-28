using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKI.Communication.Serial
{
    public enum eEvent : int
    {
        Unknown = -1,               // Unknown event
        None = 0,                   // Event trigged without cause
        Recv = 0x0001,              // Data is received on input
        RcvEv = 0x0002,             // Event character was received on input
        Send = 0x0004,              // Last character on output was sent
        CTS = 0x0008,               // The CTS signal changed state
        DSR = 0x0010,               // The DSR signal changed state
        RLSD = 0x0020,              // The RLSD signal changed state
        Break = 0x0040,             // A break was detected on input
        Error = 0x0080,             // A line-status error occurred
        Ring = 0x0100,              // A ring indicator was detected
        PrinterError = 0x0200,      // Printer error occured
        Rx80Full = 0x0400,          // Receive buffer is 80 percent full
        Event1 = 0x0800,            // Provider specific event 1
        Event2 = 0x1000,            // Provider specific event 2
    }

    public enum eBaudrate : int
    {
        Unknown = -1,               // Unknown
        Baud110 = 110,              // 110 bits/sec
        Baud300 = 300,              // 300 bits/sec
        Baud600 = 600,              // 600 bits/sec
        Baud1200 = 1200,            // 1200 bits/sec
        Baud2400 = 2400,            // 2400 bits/sec
        Baud4800 = 4800,            // 4800 bits/sec
        Baud9600 = 9600,            // 9600 bits/sec
        Baud14400 = 14400,          // 14400 bits/sec
        Baud19200 = 19200,          // 19200 bits/sec (default)
        Baud38400 = 38400,          // 38400 bits/sec
        Baud56000 = 56000,          // 56000 bits/sec
        Baud57600 = 57600,          // 57600 bits/sec
        Baud115200 = 115200,        // 115200 bits/sec
        Baud128000 = 128000,        // 128000 bits/sec
        Baud256000 = 256000,        // 256000 bits/sec
    }

    public enum eDataBits : int
    {
        Unknown = -1,               // Unknown
        Data5 = 5,                  // 5 bits per byte
        Data6 = 6,                  // 6 bits per byte
        Data7 = 7,                  // 7 bits per byte
        Data8 = 8                   // 8 bits per byte (default)
    }

    public enum eParity : int
    {
        Unknown = -1,               // Unknown
        None = 0,                   // No parity (default)
        Odd = 1,                    // Odd parity
        Even = 2,                   // Even parity
        Mark = 3,                   // Mark parity
        Space = 4                   // Space parity
    }

    public enum eStopBits : int
    {
        Unknown = -1,               // Unknown
        EStop1 = 0,                 // 1 stopbit (default)
        EStop1_5 = 1,               // 1.5 stopbit
        EStop2 = 2                  // 2 stopbits
    }

    public enum eHandshake : int
    {
        Unknown = -1,               // Unknown
        Off = 0,                    // No handshaking
        Hardware = 1,               // Hardware handshaking (RTS/CTS)
        Software = 2                // Software handshaking (XON/XOFF)
    }
    
    public enum eReadTimeout : int
    {
        Unknown = -1,               // Unknown
        Nonblocking = 0,            // Always return immediately
        Blocking = 1                // Block until everything is retrieved
    }

    public enum eError : int
    {
        Unknown = 0,                // Unknown
        RxOver = 0x0001,            // Input buffer overflow, byte lost
        Overrun = 0x0002,           // Character buffer overrun, next byte is lost
        Parity = 0x0004,            // Input parity error
        Frame = 0x0008,             // Framing error
        Break = 0x0010,             // Break condition detected
        TxFull = 0x0100,            // Output buffer full
        PTO = 0x0200,               // LPTx Timeout
        IOE = 0x0400,               // LPTx I/O Error
        DNS = 0x0800,               // LPTx Device not selected
        OOP = 0x1000,               // LPTx Out-Of-Paper
        Mode = 0x8000,              // Unsupported mode
    }

    public enum ePort : int
    {
        UnknownError = -1,          // Unknown error occurred
        Available = 0,              // Port is available
        NotAvailable = 1,           // Port is not present
        InUse = 2                   // Port is in use
    }
}
