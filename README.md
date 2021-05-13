# TrainTrain kata

This kata is a variation of the Train Reservation kata of @tpierrain and @brunoboucard (![Here's the original kata I'm referring to](https://www.youtube.com/watch?v=mZzPwt9vhHM)). It's a big kata because there's multiple Bounded Contexts (BC) and each one has a different approach. Every BC is independant, so you don't have to code everything if you want to focus on one aspect.


## Context

You work in TrainTrain company that sells softwares to manage all aspect of train travels. There's 4 departments in TrainTrain :
- Fleet : Manages all the inventory of the machines, especially locomotives and cars
- Network : Manages all the inventory of the network. Every train station, every lines and all the stations it stops by.
- Travel : Creates the travels depending on which locomotives and cars are available in a station at a specific time.
- Reservation : Manages everything about travel reservation, available seats, price, discounts...

## 