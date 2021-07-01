using OdinPlugs.OdinInject;
using OdinPlugs.OdinMvcCore.OdinInject.InjectInterface;

namespace OdinPlugs.OdinNetCore.OdinSnowFlake.SnowFlakeInterface
{
    public interface IOdinSnowFlake : IAutoInjectWithParamas
    {
        long NextId();
        string AnalyzeId(long Id);
        void InitDic();
        void ClearDic();
    }
}