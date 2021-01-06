/*===============================================================
*Project:    Library
*Developer:  Michael Girard (mgirard989@outlook.com)
*Company:    TeamTorqueEmTech
*Date:       01/04/2021 19:24
*Version:    1.5
*Description: 
*================================================================*/
/*===============================================================
*   AUDIT LOG
*
*
*================================================================*/
using System;
using System.Collections.Generic;
using System.Text;
using BarrettTorqueEm.Utilities;
using SimpleTcp;
using UnityEngine;

namespace BarrettTorqueEm.Utilities {
    public static class PacketHandler {
        private const byte ENDBYTE = (byte)255;

        private static StringBuilder StringBuilder = new StringBuilder();

        public enum PacketType {
            ERROR,
            String,
            Integer,
            Double,
            Float,
            Vector2,
            Vector3,
            Vector4,
            Quaternion,
        }

        private static byte CharToByte(char input) {
            switch (input) {
                case '0': return (byte)0;
                case '1': return (byte)1;
                case '2': return (byte)2;
                case '3': return (byte)3;
                case '4': return (byte)4;
                case '5': return (byte)5;
                case '6': return (byte)6;
                case '7': return (byte)7;
                case '8': return (byte)8;
                case '9': return (byte)9;
                case ',': return (byte)10;
                case '.': return (byte)11;
                default: return (byte)254;
            }
        }


        //Server
        public static string GetString(byte[] data, out PacketType Type) {
            Type = PacketType.ERROR;
            StringBuilder.Clear();

            for (int i = 0; i < data.Length; i++) {
                if (i == 0) {
                    Type = (PacketType)data[i];
                    continue;
                }

                switch (data[i]) {
                    case 10:
                    case 11:
                        break;
                    case 254:
                        LogHandler.LogMessage(LogLevel.Warning, "PacketHandler.cs", $"Packet had error.");
                        return "ERROR";
                    case 255:
                        //Append : Deleminator
                        break;
                    default:
                        StringBuilder.Append(data[i]);
                        break;
                }
            }
            // return Encoding.UTF8.GetString(data);
            return StringBuilder.ToString();
        }

        //CLIENT
        public static byte[] GetBytes(string Message) {
            List<byte> data = new List<byte>();
            data.Add((byte)PacketType.String);

            foreach (byte b in Encoding.UTF8.GetBytes(Message))
                data.Add(b);

            data.Add(ENDBYTE);

            return data.ToArray();
        }

        public static byte[] GetBytes(int Number) {
            char[] temp = Number.ToString().ToCharArray();
            byte[] b = new byte[temp.Length + 2];

            b[0] = (byte)PacketType.Integer;

            for (int i = 0; i < temp.Length; i++) {
                b[i + 1] = CharToByte(temp[i]);
            }

            b[temp.Length + 1] = ENDBYTE;

            return b;
        }

        public static byte[] GetBytes(double Number) {
            char[] temp = Number.ToString().ToCharArray();
            byte[] b = new byte[temp.Length + 2];

            b[0] = (byte)PacketType.Integer;

            for (int i = 0; i < temp.Length; i++) {
                b[i + 1] = CharToByte(temp[i]);
            }

            b[temp.Length + 1] = ENDBYTE;

            return b;
        }

        public static byte[] GetBytes(Vector3 Transform) {
            return Encoding.UTF8.GetBytes(Transform.x.ToString() + Transform.y.ToString() + Transform.z.ToString());
        }
        public static byte[] GetBytes(Vector2 Transform) {
            return Encoding.UTF8.GetBytes(Transform.x.ToString() + Transform.y.ToString());
        }
        public static byte[] GetBytes(Quaternion Rotation) {
            return Encoding.UTF8.GetBytes(Rotation.x.ToString() + Rotation.y.ToString() + Rotation.z.ToString() + Rotation.w.ToString());
        }
        public static byte[] GetBytes(Vector4 Rotation) {
            return Encoding.UTF8.GetBytes(Rotation.x.ToString() + Rotation.y.ToString() + Rotation.z.ToString() + Rotation.w.ToString());
        }
    }
}