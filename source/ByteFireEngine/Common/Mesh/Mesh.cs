using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByteFireEngine.Helper;
using OpenTK.Graphics.OpenGL4;
using static OpenTK.Graphics.OpenGL.GL;

//using OpenTK.Graphics.ES11;
//using static OpenTK.Graphics.ES30.GL;
//using DrawElementsType = OpenTK.Graphics.ES30.DrawElementsType;

namespace ByteFireEngine.Common.Mesh;

public class Mesh
{
    private readonly int vao;
    private readonly int vbo;
    private readonly int ibo;
    private readonly int indexCount;

    public Mesh(float[] vertexData, int[] indexData, VertexAttribute[] attributes)
    {
        indexCount = indexData.Length;

        // generate IDs
        vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);

        vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertexData.Length * sizeof(float), vertexData, BufferUsageHint.StaticDraw);


        ibo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indexData.Length * sizeof(int), indexData, BufferUsageHint.StaticDraw);

        attributes.ForEachIndexed((attribute, index) =>
        {
            GL.EnableVertexAttribArray(index);
            GL.VertexAttribPointer(index, attribute.Size, attribute.Type, false, attribute.Stride, attribute.Offset);
        });

        //Unbind
        GL.BindVertexArray(0);
        GL.BindBuffer(BufferTarget.ArrayBuffer,0);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer,0);
    }

    public Mesh(float[] vertexData, VertexAttribute[] attributes)
    {
        // generate IDs
        vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);

        vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertexData.Length * sizeof(float), vertexData, BufferUsageHint.StaticDraw);

        var attributeSizeSum = 0;

        attributes.ForEachIndexed((attribute, index) =>
        {
            GL.EnableVertexAttribArray(index);
            GL.VertexAttribPointer(index, attribute.Size, attribute.Type, false, attribute.Stride, attribute.Offset);
            attributeSizeSum += attribute.Size;
        });

        if (attributeSizeSum != 0)
            indexCount = vertexData.Length / attributeSizeSum;

        //Unbind
        GL.BindVertexArray(0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
    }

    public void Render()
    {
        if (ibo == 0)
        {
            GL.BindVertexArray(vao);
            GL.EnableVertexAttribArray(0);
            GL.DrawArrays(PrimitiveType.Triangles, 0, indexCount);
            GL.BindVertexArray(0);
            GL.DisableVertexAttribArray(0);
        }
        else
        {
            GL.BindVertexArray(vao);
            GL.DrawElements(PrimitiveType.Triangles, indexCount, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }
    }

    public void CleanUp()
    {
        if (ibo != 0) GL.DeleteBuffer(ibo);
        if (vbo != 0) GL.DeleteBuffer(vbo);
        if (vao != 0) GL.DeleteVertexArray(ibo);
    }


}