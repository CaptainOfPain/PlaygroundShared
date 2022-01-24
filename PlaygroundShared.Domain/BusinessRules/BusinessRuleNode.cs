namespace PlaygroundShared.Domain.BusinessRules;

public class BusinessRuleNode
{
    public IBusinessRule Left { get; }
    public BusinessRuleNode Right { get; private set; }
    public BusinessRuleNodeType? Type { get; private set; }
        
    private List<string> _messages = new ();
    public IEnumerable<string> Messages => _messages;

    public BusinessRuleNode(IBusinessRule left)
    {
        Left = left;
    }

    public void SetRight(BusinessRuleNode right, BusinessRuleNodeType type)
    {
        Type = type;
        Right = right;
    }

    public bool IsNodeBroken()
    {
        var isBroken = false;
        switch (Type)
        {
            case BusinessRuleNodeType.And:
                isBroken = Left.IsBroken() && Right.IsNodeBroken();
                break;
            case BusinessRuleNodeType.Or:
                isBroken = Left.IsBroken() || Right.IsNodeBroken();
                break;
            default:
                isBroken = Left.IsBroken();
                break;
        }
            
        if(Right != null && isBroken) _messages.AddRange(Right.Messages);
        if(Left.IsBroken()) _messages.Add(Left.Message);

        return isBroken;
    }
}