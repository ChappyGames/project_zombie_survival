using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sent from server to client. 
/// </summary>
public enum ServerPackets {
    WELCOME = 1,
    ENTITY_SPAWN,
    PLAYER_SPAWN,
    ENTITY_POS,
    ENTITY_ROTATION,
    PLAYER_DISCONNECTED,
    ENTITY_HEALTH,
    ENTITY_RESPAWNED,
    PLAYER_WEAPON_EQUIPPED,
    ENTITY_ATTACK,
    PLAYER_WEAPON_RELOADED,
    INVENTORY_ITEM_ADDED,
    INVENTORY_ITEM_REMOVED,
    INVENTORY_ITEM_USED
}

/// <summary>
/// Sent from client to server.
/// </summary>
public enum ClientPackets {
    WELCOME_RECEIVED = 1,
    PLAYER_MOVEMENT,
    PLAYER_ATTACK,
    PLAYER_ITEM_USED
}

public class Packet : IDisposable {
    private List<byte> buffer;
    private byte[] readableBuffer;
    private int readPos;

    private bool disposed = false;

    /// <summary>
    /// Creates a new empty packet (without an ID).
    /// </summary>
    public Packet() {
        buffer = new List<byte>(); // Initialize buffer
        readPos = 0; // Set readPos to 0
    }

    /// <summary>
    /// Creates a new packet with a given ID. Used for sending.
    /// </summary>
    /// <param name="aID">The packet ID.</param>
    public Packet(int aID) {
        buffer = new List<byte>(); // Initialize buffer
        readPos = 0; // Set readPos to 0

        Write(aID); // Write packet id to the buffer
    }

    /// <summary>
    /// Creates a packet from which data can be read. Used for receiving.
    /// </summary>
    /// <param name="aData">The bytes to add to the packet.</param>
    public Packet(byte[] aData) {
        buffer = new List<byte>(); // Initialize buffer
        readPos = 0; // Set readPos to 0

        SetBytes(aData);
    }

    #region Functions
    /// <summary>
    /// Sets the packet's content and prepares it to be read.
    /// </summary>
    /// <param name="aData">The bytes to add to the packet.</param>
    public void SetBytes(byte[] aData) {
        Write(aData);
        readableBuffer = buffer.ToArray();
    }

    /// <summary>
    /// Inserts the length of the packet's content at the start of the buffer.
    /// </summary>
    public void WriteLength() {
        buffer.InsertRange(0, BitConverter.GetBytes(buffer.Count)); // Insert the byte length of the packet at the very beginning
    }

    /// <summary>
    /// Inserts the given int at the start of the buffer.
    /// </summary>
    /// <param name="aValue">The int to insert.</param>
    public void InsertInt(int aValue) {
        buffer.InsertRange(0, BitConverter.GetBytes(aValue)); // Insert the int at the start of the buffer
    }

    /// <summary>
    /// Gets the packet's content in array form.
    /// </summary>
    /// <returns>Returns the readable buffer.</returns>
    public byte[] ToArray() {
        readableBuffer = buffer.ToArray();
        return readableBuffer;
    }

    /// <summary>
    /// Gets the length of the packet's content.
    /// </summary>
    /// <returns>Returns the length of the packet's content.</returns>
    public int Length() {
        return buffer.Count; // Return the length of the buffer
    }

    /// <summary>
    /// Gets the length of the unread data contained in the packet.
    /// </summary>
    /// <returns>Returns the remaining (unread) length.</returns>
    public int UnreadLength() {
        return Length() - readPos; // Return the remaining length (unread)
    }

    /// <summary>
    /// Resets the packet instance to allow it to be reused.
    /// </summary>
    /// <param name="aShouldReset">Whether or not to reset the packet.</param>
    public void Reset(bool aShouldReset = true) {
        if (aShouldReset == true) {
            buffer.Clear(); // Clear the buffer
            readableBuffer = null;
            readPos = 0; //Reset readPos
        }
        else {
            readPos -= 4; // "Unread" the last read int
        }
    }
    #endregion

    #region Write Data
    /// <summary>
    /// Adds a byte to the packet.
    /// </summary>
    /// <param name="aValue">The byte to add.</param>
    public void Write(byte aValue) {
        buffer.Add(aValue);
    }

    /// <summary>
    /// Adds an array of bytes to the packet.
    /// </summary>
    /// <param name="aValue">The byte array to add.</param>
    public void Write(byte[] aValue) {
        buffer.AddRange(aValue);
    }

    /// <summary>
    /// Adds a short to the packet.
    /// </summary>
    /// <param name="aValue">The short to add.</param>
    public void Write(short aValue) {
        buffer.AddRange(BitConverter.GetBytes(aValue));
    }

    /// <summary>
    /// Adds an int to the packet.
    /// </summary>
    /// <param name="aValue">The int to add.</param>
    public void Write(int aValue) {
        buffer.AddRange(BitConverter.GetBytes(aValue));
    }

    /// <summary>
    /// Adds a long to the packet.
    /// </summary>
    /// <param name="aValue">The long to add.</param>
    public void Write(long aValue) {
        buffer.AddRange(BitConverter.GetBytes(aValue));
    }

    /// <summary>
    /// Adds a float to the packet.
    /// </summary>
    /// <param name="aValue">The float to add.</param>
    public void Write(float aValue) {
        buffer.AddRange(BitConverter.GetBytes(aValue));
    }

    /// <summary>
    /// Adds a bool to the packet.
    /// </summary>
    /// <param name="aValue">The bool to add.</param>
    public void Write(bool aValue) {
        buffer.AddRange(BitConverter.GetBytes(aValue));
    }

    /// <summary>
    /// Adds a string to the packet.
    /// </summary>
    /// <param name="aValue">The string to add.</param>
    public void Write(string aValue) {
        Write(aValue.Length); // Add the length of the string to the packet
        buffer.AddRange(Encoding.ASCII.GetBytes(aValue)); // Add the string itself
    }

    /// <summary>
    /// Adds a Vector3 to the packet.
    /// </summary>
    /// <param name="aValue">The Vector3 to add.</param>
    public void Write(Vector3 aValue) {
        Write(aValue.x);
        Write(aValue.y);
        Write(aValue.z);
    }

    /// <summary>
    /// Adds a Quaternion to the packet.
    /// </summary>
    /// <param name="aValue">The Quaternion to add.</param>
    public void Write(Quaternion aValue) {
        Write(aValue.x);
        Write(aValue.y);
        Write(aValue.z);
        Write(aValue.w);
    }
    #endregion

    #region Read Data
    /// <summary>
    /// Reads a byte from the packet.
    /// </summary>
    /// <param name="aMoveReadPos"> Whether or not to move the buffer's read position.</param>
    /// <returns>Returns the byte.</returns>
    public byte ReadByte(bool aMoveReadPos = true) {
        if (buffer.Count > readPos) {
            // If there are unread bytes
            byte lValue = readableBuffer[readPos]; // Get the byte at readPos' position
            if (aMoveReadPos == true) {
                readPos += 1;
            }
            return lValue;
        }
        else {
            throw new Exception("Could not read value of type 'byte'!");
        }
    }

    /// <summary>
    /// Reads an array of bytes from the packet.
    /// </summary>
    /// <param name="aLength">The length of the byte array.</param>
    /// <param name="aMoveReadPos">Whether or not to move the buffer's read position.</param>
    public byte[] ReadBytes(int aLength, bool aMoveReadPos = true) {
        if (buffer.Count > readPos) {
            // If there are unread bytes
            byte[] lValue = buffer.GetRange(readPos, aLength).ToArray(); // Get the bytes at readPos' position with a range of _length
            if (aMoveReadPos) {
                // If _moveReadPos is true
                readPos += aLength; // Increase readPos by _length
            }
            return lValue; // Return the bytes
        }
        else {
            throw new Exception("Could not read value of type 'byte[]'!");
        }
    }

    /// <summary>
    /// Reads a short from the packet.
    /// </summary>
    /// <param name="aMoveReadPos">Whether or not to move the buffer's read position.</param>
    public short ReadShort(bool aMoveReadPos = true) {
        if (buffer.Count > readPos) {
            // If there are unread bytes
            short lValue = BitConverter.ToInt16(readableBuffer, readPos); // Convert the bytes to a short
            if (aMoveReadPos) {
                // If _moveReadPos is true and there are unread bytes
                readPos += 2; // Increase readPos by 2
            }
            return lValue; // Return the short
        }
        else {
            throw new Exception("Could not read value of type 'short'!");
        }
    }

    /// <summary>
    /// Reads an int from the packet.
    /// </summary>
    /// <param name="aMoveReadPos">Whether or not to move the buffer's read position.</param>
    public int ReadInt(bool aMoveReadPos = true) {
        if (buffer.Count > readPos) {
            // If there are unread bytes
            int lValue = BitConverter.ToInt32(readableBuffer, readPos); // Convert the bytes to an int
            if (aMoveReadPos) {
                // If _moveReadPos is true
                readPos += 4; // Increase readPos by 4
            }
            return lValue; // Return the int
        }
        else {
            throw new Exception("Could not read value of type 'int'!");
        }
    }

    /// <summary>
    /// Reads a long from the packet.
    /// </summary>
    /// <param name="aMoveReadPos">Whether or not to move the buffer's read position.</param>
    public long ReadLong(bool aMoveReadPos = true) {
        if (buffer.Count > readPos) {
            // If there are unread bytes
            long lValue = BitConverter.ToInt64(readableBuffer, readPos); // Convert the bytes to a long
            if (aMoveReadPos) {
                // If _moveReadPos is true
                readPos += 8; // Increase readPos by 8
            }
            return lValue; // Return the long
        }
        else {
            throw new Exception("Could not read value of type 'long'!");
        }
    }

    /// <summary>
    /// Reads a float from the packet.
    /// </summary>
    /// <param name="aMoveReadPos">Whether or not to move the buffer's read position.</param>
    public float ReadFloat(bool aMoveReadPos = true) {
        if (buffer.Count > readPos) {
            // If there are unread bytes
            float lValues = BitConverter.ToSingle(readableBuffer, readPos); // Convert the bytes to a float
            if (aMoveReadPos) {
                // If _moveReadPos is true
                readPos += 4; // Increase readPos by 4
            }
            return lValues; // Return the float
        }
        else {
            throw new Exception("Could not read value of type 'float'!");
        }
    }

    /// <summary>
    /// Reads a bool from the packet.
    /// </summary>
    /// <param name="aMoveReadPos">Whether or not to move the buffer's read position.</param>
    public bool ReadBool(bool aMoveReadPos = true) {
        if (buffer.Count > readPos) {
            // If there are unread bytes
            bool lValues = BitConverter.ToBoolean(readableBuffer, readPos); // Convert the bytes to a bool
            if (aMoveReadPos) {
                // If _moveReadPos is true
                readPos += 1; // Increase readPos by 1
            }
            return lValues; // Return the bool
        }
        else {
            throw new Exception("Could not read value of type 'bool'!");
        }
    }

    /// <summary>
    /// Reads a string from the packet.
    /// </summary>
    /// <param name="aMoveReadPos">Whether or not to move the buffer's read position.</param>
    public string ReadString(bool aMoveReadPos = true) {
        try {
            int lLength = ReadInt(); // Get the length of the string
            string lValue = Encoding.ASCII.GetString(readableBuffer, readPos, lLength); // Convert the bytes to a string
            if (aMoveReadPos && lValue.Length > 0) {
                // If _moveReadPos is true string is not empty
                readPos += lLength; // Increase readPos by the length of the string
            }
            return lValue; // Return the string
        }
        catch {
            throw new Exception("Could not read value of type 'string'!");
        }
    }

    /// <summary>
    /// Reads a Vector3 from the packet.
    /// </summary>
    /// <param name="aMoveReadPos">Whether or not to move the buffer's read position.</param>
    public Vector3 ReadVector3(bool aMoveReadPos = true) {
        return new Vector3(ReadFloat(aMoveReadPos), ReadFloat(aMoveReadPos), ReadFloat(aMoveReadPos));
    }

    /// <summary>
    /// Reads a Quaternion from the packet.
    /// </summary>
    /// <param name="aMoveReadPos">Whether or not to move the buffer's read position.</param>
    public Quaternion ReadQuaternion(bool aMoveReadPos = true) {
        return new Quaternion(ReadFloat(aMoveReadPos), ReadFloat(aMoveReadPos), ReadFloat(aMoveReadPos), ReadFloat(aMoveReadPos));
    }
    #endregion

    protected virtual void Dispose(bool aDisposing) {
        if (disposed == false) {
            if (aDisposing == true) {
                buffer = null;
                readableBuffer = null;
                readPos = 0;
            }

            disposed = true;
        }
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

}
