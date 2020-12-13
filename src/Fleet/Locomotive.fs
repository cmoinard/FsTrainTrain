namespace Fleet

type LocomotiveId = int
type [<Measure>] ton

[<NoEquality; NoComparison>]
type Locomotive = {
    Id: LocomotiveId
    Brand: string
    Model: string
    Weight: decimal<ton>
    MaxTraction: decimal<ton>
}

type NewLocomotive = {
    Brand: string
    Model: string
    Weight: decimal<ton>
    MaxTraction: decimal<ton>
}