import { Result } from 'functional-sharp';
import { Customer } from '../types/customer';

const API_URL = '/api/customers';

export const customerService = {
    getAll: async (): Promise<Result<Customer[]>> => {
        try {
            const response = await fetch(API_URL);
            if (!response.ok) {
                return Result.failure(`Failed to fetch customers: ${response.statusText}`);
            }
            const data = await response.json();
            return Result.success(data);
        } catch (error) {
            return Result.failure(`Error fetching customers: ${error}`);
        }
    },

    getById: async (id: string): Promise<Result<Customer>> => {
        try {
            const response = await fetch(`${API_URL}/${id}`);
            if (!response.ok) {
                return Result.failure(`Failed to fetch customer: ${response.statusText}`);
            }
            const data = await response.json();
            return Result.success(data);
        } catch (error) {
            return Result.failure(`Error fetching customer: ${error}`);
        }
    },

    create: async (customer: Omit<Customer, 'id'>): Promise<Result<Customer>> => {
        try {
            const response = await fetch(API_URL, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(customer),
            });
            if (!response.ok) {
                return Result.failure(`Failed to create customer: ${response.statusText}`);
            }
            const data = await response.json();
            return Result.success(data);
        } catch (error) {
            return Result.failure(`Error creating customer: ${error}`);
        }
    },

    update: async (id: string, customer: Partial<Customer>): Promise<Result<Customer>> => {
        try {
            const response = await fetch(`${API_URL}/${id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(customer),
            });
            if (!response.ok) {
                return Result.failure(`Failed to update customer: ${response.statusText}`);
            }
            const data = await response.json();
            return Result.success(data);
        } catch (error) {
            return Result.failure(`Error updating customer: ${error}`);
        }
    },

    delete: async (id: string): Promise<Result<void>> => {
        try {
            const response = await fetch(`${API_URL}/${id}`, {
                method: 'DELETE',
            });
            if (!response.ok) {
                return Result.failure(`Failed to delete customer: ${response.statusText}`);
            }
            return Result.success(undefined);
        } catch (error) {
            return Result.failure(`Error deleting customer: ${error}`);
        }
    }
}; 