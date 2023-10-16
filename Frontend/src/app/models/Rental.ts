import { Car } from "./Car";
import { User } from "./User";

export interface Rental {
    id: number,
    carId: number,
    car: Car,
    userId: number,
    user: User,
    rentDurationDays: number,
    totalCost: number,
    rentalStartDate: Date,
    rentalEndDate: Date,
    requestForReturn: boolean
}