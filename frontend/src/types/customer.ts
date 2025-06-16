export interface Address {
    street: string;
    city: string;
    postalCode: string;
    country: string;
}

export interface Customer {
    id: string;
    companyName: string;
    vatNumber: string;
    email: string;
    address: Address;
    isActive: boolean;
}

export interface CreateCustomerDto {
    companyName: string;
    vatNumber: string;
    email: string;
    address: Address;
}

export interface UpdateCustomerDto {
    companyName?: string;
    vatNumber?: string;
    email?: string;
    address?: Address;
    isActive?: boolean;
} 