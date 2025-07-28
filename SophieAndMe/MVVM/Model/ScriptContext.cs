using SophieAndMe.MVVM.View;
using SophieAndMe.MVVM.ViewModel;

namespace SophieAndMe.MVVM.Model;

public class ScriptContext
{
    public VCustomModel VM { get; }

    public ScriptContext(VCustomModel vm)
    {
        VM = vm;
    }

    public void FirstLayer(object matier) => VM.FirstLayer(matier);
    public void SecondeLayer(object matier) => VM.SecondeLayer(matier);
    public void CreatedLogic() => VM.CreatedLogic();
}
