namespace proiect_dotnet.Models
{
    public class GroupMembership
    {
        public int Id {get; set;} //primary key of the table -> the row where the request was made

        public int GroupId {get; set;} // says which at which group is the request made
        public StudyGroup? Group {get; set;}

        public string UserId {get; set;} = string.Empty;
        public ApplicationUser? User {get; set;}

        public MembershipStatus Status {get; set;} = MembershipStatus.PENDING;
        public DateTime RequestedAt {get; set;} = DateTime.UtcNow;
    }
}