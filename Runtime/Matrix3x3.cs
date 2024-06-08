﻿using System;
using UnityEngine;

namespace MeshTransformer
{
    public struct Matrix3x3
    {
        public float m00, m10, m20;
        public float m01, m11, m21;
        public float m02, m12, m22;
        
        public float this[int row, int column]
        { 
            get => this[row + column * 3]; 
            set => this[row + column * 3] = value;
        }
        
        public float this[int index]
        {
            get
            {
                return index switch
                {
                    0 => m00,
                    1 => m10,
                    2 => m20,
                    3 => m01,
                    4 => m11,
                    5 => m21,
                    6 => m02,
                    7 => m12,
                    8 => m22,
                    _ => throw new IndexOutOfRangeException("Invalid matrix index!")
                };
            }
            set
            {
                switch (index)
                {
                    case 0: m00 = value; break;
                    case 1: m10 = value; break;
                    case 2: m20 = value; break;
                    case 3: m01 = value; break;
                    case 4: m11 = value; break;
                    case 5: m21 = value; break;
                    case 6: m02 = value; break;
                    case 7: m12 = value; break;
                    case 8: m22 = value; break;
                    default:
                        throw new IndexOutOfRangeException("Invalid matrix index!");
                }
            }
        }

        public static Matrix3x3 operator *(Matrix3x3 lhs, Matrix3x3 rhs)
        {
            Matrix3x3 matrix3x3;
            matrix3x3.m00 = (lhs.m00 * rhs.m00) + (lhs.m01 * rhs.m10) + (lhs.m02 * rhs.m20);
            matrix3x3.m01 = (lhs.m00 * rhs.m01) + (lhs.m01 * rhs.m11) + (lhs.m02 * rhs.m21);
            matrix3x3.m02 = (lhs.m00 * rhs.m02) + (lhs.m01 * rhs.m12) + (lhs.m02 * rhs.m22);
      
            matrix3x3.m10 = (lhs.m10 * rhs.m00) + (lhs.m11 * rhs.m10) + (lhs.m12 * rhs.m20);
            matrix3x3.m11 = (lhs.m10 * rhs.m01) + (lhs.m11 * rhs.m11) + (lhs.m12 * rhs.m21);
            matrix3x3.m12 = (lhs.m10 * rhs.m02) + (lhs.m11 * rhs.m12) + (lhs.m12 * rhs.m22);
      
            matrix3x3.m20 = (lhs.m20 * rhs.m00) + (lhs.m21 * rhs.m10) + (lhs.m22 * rhs.m20);
            matrix3x3.m21 = (lhs.m20 * rhs.m01) + (lhs.m21 * rhs.m11) + (lhs.m22 * rhs.m21);
            matrix3x3.m22 = (lhs.m20 * rhs.m02) + (lhs.m21 * rhs.m12) + (lhs.m22 * rhs.m22);

            return matrix3x3;
        }

        public static Vector3 operator *(Matrix3x3 lhs, Vector3 vector)
        {
            Vector3 vector3;

            vector3.x = lhs.m00 * vector.x + lhs.m01 * vector.y + lhs.m02 * vector.z;
            vector3.y = lhs.m10 * vector.x + lhs.m11 * vector.y + lhs.m12 * vector.z;
            vector3.z = lhs.m20 * vector.x + lhs.m21 * vector.y + lhs.m22 * vector.z;
      
            return vector3;
        }
    
        public static bool operator ==(Matrix3x3 lhs, Matrix3x3 rhs)
        {
            return lhs.GetColumn(0) == rhs.GetColumn(0) && lhs.GetColumn(1) == rhs.GetColumn(1) && lhs.GetColumn(2) == rhs.GetColumn(2) && lhs.GetColumn(3) == rhs.GetColumn(3);
        }

        public static bool operator !=(Matrix3x3 lhs, Matrix3x3 rhs) => !(lhs == rhs);
        
        public Vector3 GetColumn(int index)
        {
            return index switch
            {
                0 => new Vector3(m00, m10, m20),
                1 => new Vector3(m01, m11, m21),
                2 => new Vector3(m02, m12, m22),
                _ => throw new IndexOutOfRangeException("Invalid column index!")
            };
        }
        
        public Vector3 GetRow(int index)
        {
            return index switch
            {
                0 => new Vector3(m00, m01, m02),
                1 => new Vector3(m10, m11, m12),
                2 => new Vector3(m20, m21, m22),
                _ => throw new IndexOutOfRangeException("Invalid row index!")
            };
        }
        
        public void SetColumn(int index, Vector3 column)
        {
            this[0, index] = column.x;
            this[1, index] = column.y;
            this[2, index] = column.z;
        }
        
        public void SetRow(int index, Vector3 row)
        {
            this[index, 0] = row.x;
            this[index, 1] = row.y;
            this[index, 2] = row.z;
        }
    }
}