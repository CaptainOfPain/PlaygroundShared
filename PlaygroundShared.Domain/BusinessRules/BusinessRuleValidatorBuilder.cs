namespace PlaygroundShared.Domain.BusinessRules;

public class BusinessRuleValidatorBuilder : IBusinessRuleValidatorBuilder
{
    private BusinessRuleNode _parentNode;

    public BusinessRuleValidatorBuilder(IBusinessRule businessRule)
    {
        _parentNode = new BusinessRuleNode(businessRule);
    }
        
    public IBusinessRuleValidatorBuilder And(IBusinessRule businessRule)
    {
        var node = new BusinessRuleNode(businessRule);
        node.SetRight(_parentNode, BusinessRuleNodeType.And);

        _parentNode = node;
        return this;
    }

    public IBusinessRuleValidatorBuilder Or(IBusinessRule businessRule)
    {
        var node = new BusinessRuleNode(businessRule);
        node.SetRight(_parentNode, BusinessRuleNodeType.And);

        _parentNode = node;
        return this;
    }

    public (bool Result, IEnumerable<string> Messages) IsBroken() => (_parentNode.IsNodeBroken(), _parentNode.Messages);
}