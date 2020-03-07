using System;
using System.IO;

namespace Fast.Buffers
{
    public class ByteBuffer
    {
        private byte[] buffer = new byte[0];

        private int write_pointer = 0;

        private int read_pointer = 0;

        private int int_size = 0;

        public ByteBuffer()
        {
            int_size = BitConverter.GetBytes(int.MaxValue).Length;
        }

        public ByteBuffer(byte[] buffer)
        {
            int_size = BitConverter.GetBytes(int.MaxValue).Length;

            if (buffer != null)
            {
                if (buffer.Length > 0)
                {
                    this.buffer = buffer;
                    write_pointer = buffer.Length;
                }
            }
        }

        public void MoveWritePointerToPos(int new_pos)
        {
            write_pointer = new_pos;
        }

        public void MoveReadPointerToPos(int new_pos)
        {
            read_pointer = new_pos;
        }

        public void Add(Int32 val)
        {
            byte[] bytes = BitConverter.GetBytes(val);

            Add(bytes);
        }

        public void Add(float val)
        {
            byte[] bytes = BitConverter.GetBytes(val);

            Add(bytes);
        }

        public void Add(double val)
        {
            byte[] bytes = BitConverter.GetBytes(val);

            Add(bytes);
        }

        public void Add(byte[] val)
        {
            if (val != null)
            {
                if (val.Length > 0)
                {
                    int start_write = write_pointer;

                    int end_write = write_pointer + int_size + val.Length;

                    Reserve(end_write);

                    // -------

                    byte[] size_bytes = BitConverter.GetBytes(val.Length);

                    size_bytes.CopyTo(buffer, write_pointer);

                    write_pointer += int_size;

                    val.CopyTo(buffer, write_pointer);

                    write_pointer += val.Length;
                }
            }
        }

        private byte[] GetBytes()
        {
            byte[] ret = new byte[0];

            int next_bytes_size = 0;

            if (read_pointer < buffer.Length)
            {
                next_bytes_size = BitConverter.ToInt32(buffer, read_pointer);

                read_pointer += int_size;
            }

            if (read_pointer < buffer.Length)
            {
                ret = new byte[next_bytes_size];

                Array.Copy(buffer, read_pointer, ret, 0, next_bytes_size);

                read_pointer += next_bytes_size;
            }

            return ret;
        }

        public Int32 GetInt32()
        {
            Int32 ret = 0;

            byte[] bytes = GetBytes();

            ret = BitConverter.ToInt32(bytes, 0);

            return ret;
        }

        public float GetFloat()
        {
            float ret = 0;

            byte[] bytes = GetBytes();

            ret = (float)BitConverter.ToDouble(bytes, 0);

            return ret;
        }

        public double GetDouble()
        {
            double ret = 0;

            byte[] bytes = GetBytes();

            ret = BitConverter.ToDouble(bytes, 0);

            return ret;
        }

        public void Reserve(int bytes)
        {
            if(Size < bytes)
            {
                byte[] new_buffer = new byte[bytes];

                buffer.CopyTo(new_buffer, 0);

                buffer = new_buffer;
            }
        }

        public void Expand(int bytes)
        {
            int new_size = Size + bytes;

            Reserve(new_size);
        }

        public int Size
        {
            get { return buffer.Length; }
        }

        public byte[] Data
        {
            get { return buffer; }
        }

        public int WritePointer
        {
            get { return write_pointer; }
        }
    }
}
