using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace ByteFireEngine.Common.Mesh;

/// <summary>
/// 
/// </summary>
/// <param name="Size">Number of components of this attribute</param>
/// <param name="Type">Type of this attribute</param>
/// <param name="Stride">Size in bytes of lastTime whole vertex</param>
/// <param name="Offset">Offset in bytes from the beginning of the vertex to the location of this attribute data</param>
public record  VertexAttribute(int Size, VertexAttribPointerType Type, int Stride, int Offset);