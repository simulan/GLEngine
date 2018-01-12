using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLProgram.Core.Render.Common {
    /*
     * Represent a component for buffering models -> (cast) (object)
     * And retrieving them fast (covariance)
     */
    public interface BufferAdapter<out T1,out T2> {
        int Buffer(Object modelObject);
        T2 GetHandle(int modelKey);
        T1 GetModel(int modelKey);
        int EnableAttributes(int modelKey,int offset);
        int DisableAttributes(int offset);
        void DisposeBuffers();
    }
}
