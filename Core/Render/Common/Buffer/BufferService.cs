using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLProgram.Core.Loaders.Files;

namespace UMLProgram.Core.Render.Common {
    public class BufferService {
        private Dictionary<Type, BufferAdapter<Object,Object>> adapters = new Dictionary<Type, BufferAdapter<Object,Object>>();

        public BufferService() { }
        public BufferService(bool addDefaultAdapters) {
            if (addDefaultAdapters) {
                AddBufferAdapter(typeof(IndexedD3Model), new IndexedD3ModelBufferAdapter());
                AddBufferAdapter(typeof(IndexedD3Model2), new IndexedD3Model2BufferAdapter());
            }
        }
        public void AddBufferAdapter(Type t,BufferAdapter<object,object> newAdapter) { adapters.Add(t,newAdapter); }
        public int Buffer<T>(T model) { return adapters[typeof(T)].Buffer(model); }
        public T GetModel<T>(int modelKey) { return (T) adapters[typeof(T)].GetModel(modelKey); }
        public T GetHandle<T>(int modelKey,Type modelType) { return (T) adapters[modelType].GetHandle(modelKey); }
        public int EnableAttributes<T>(int modelKey,int offset) { return adapters[typeof(T)].EnableAttributes(modelKey,offset); }
        public int DisableAttributes<T>(int offset) { return adapters[typeof(T)].DisableAttributes(offset); }
        public void DisposeBuffers() {
            foreach (BufferAdapter<Object, Object> adapter in adapters.Values) adapter.DisposeBuffers();
        }
    }
}
