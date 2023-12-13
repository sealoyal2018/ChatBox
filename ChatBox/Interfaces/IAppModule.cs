namespace ChatBox.Interfaces;

public interface IAppModule
{
    /// <summary>
    /// 图标
    /// </summary>
    string Icon { get; }
    
    /// <summary>
    /// 显示名称
    /// </summary>
    string DisplayName { get; }
    
    /// <summary>
    /// 排序
    /// </summary>
    int Sort { get; }
}