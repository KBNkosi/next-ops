using JobCommandCenter.Enums;

namespace JobCommandCenter.DTOs.Contacts
{
public class UpdateRelationshipStatusRequest
{
    public RelationshipStatus RelationshipStatus { get; set; } = RelationshipStatus.NotContacted;
}
}
