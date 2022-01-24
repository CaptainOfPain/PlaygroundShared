namespace PlaygroundShared.Domain.BusinessRules;

public interface IBusinessRuleValidatorBuilder
{
    IBusinessRuleValidatorBuilder And(IBusinessRule businessRule);
    IBusinessRuleValidatorBuilder Or(IBusinessRule businessRule);
    public (bool Result, IEnumerable<string> Messages) IsBroken();
}