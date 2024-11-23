using System;
using System.Reflection;
using System.Runtime.Loader;

namespace ProyectoCalidadSoftware.Extension
{
    public class CustomAssemblyLoadContext : AssemblyLoadContext
    {
        public IntPtr LoadUnmanagedLibrary(string absolutePath)
        {
            // Llama al método base para cargar la DLL no administrada
            return LoadUnmanagedDllFromPath(absolutePath);
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            // Implementación personalizada si es necesario
            return IntPtr.Zero;
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            // Implementación personalizada si es necesario
            return null;
        }
    }
}
