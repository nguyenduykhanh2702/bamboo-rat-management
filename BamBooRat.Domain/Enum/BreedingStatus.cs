public enum BreedingStatus
{
    Breeding,       // đang phối
    Pregnant,       // đã đậu
    WaitingBirth,   // gần đẻ
    OffspringAlive, // con sống
    OffspringDead,  // con chết
    BirthSuccessful, // đã đẻ
    Failed,          // thất bại
    Separated,       // đã tách
    Cancelled,       // đã hủy
    FailedBecauseOfDeath // thất bại do rat chết
}