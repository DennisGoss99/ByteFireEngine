using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByteFireEngine.Common.Mesh.DistinctModelLoader;
using OpenTK.Graphics.ES20;

namespace ByteFireEngine.Common.Mesh;

public static class ModelLoader
{
    public static Mesh LoadFile(string path){
        string extension = Path.GetExtension(path).ToLower();

        return extension switch
        {
            ".obj" => LoadObjFile(path),
            ".stl" => LoadStlFile(path),
            _ => throw new ArgumentException($"File extension '{extension}' is not supported"),
        };
    }

    public static Mesh LoadObjFile(string path)
    {
        return ObjModelLoader.LoadObjFile(path);
    }

    public static Mesh LoadStlFile(string path)
    {
        char[] buffer = new char[5];

        using StreamReader reader = new StreamReader(path);
        reader.Read(buffer, 0, 5);
        reader.Close();

        if ("solid".Equals(new string(buffer), StringComparison.Ordinal))
            return StlModelLoader.LoadNonBinaryFile(path);

        return StlModelLoader.LoadBinaryFile(path);
    }

}
