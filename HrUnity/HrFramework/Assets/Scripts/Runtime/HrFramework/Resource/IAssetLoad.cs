using System.Collections;
using System.Collections.Generic;

namespace Hr
{
    public interface IAssetLoad
    {
        /// <summary>
        ///   同步加载
        /// </summary>
        void LoadSync();

        /// <summary>
        ///   异步加载
        /// </summary>
        IEnumerator LoadAsync();
    }
}
