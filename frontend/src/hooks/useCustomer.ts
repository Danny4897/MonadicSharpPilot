import { useState, useCallback } from 'react';
import { Result } from 'functional-sharp';
import { Customer, CreateCustomerDto, UpdateCustomerDto } from '../types/customer';
import { customerService } from '../services/customerService';

export const useCustomer = () => {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const getAll = useCallback(async (): Promise<Result<Customer[]>> => {
        setLoading(true);
        setError(null);
        try {
            const result = await customerService.getAll();
            if (result.isFailure) {
                setError(result.error);
            }
            return result;
        } finally {
            setLoading(false);
        }
    }, []);

    const getById = useCallback(async (id: string): Promise<Result<Customer>> => {
        setLoading(true);
        setError(null);
        try {
            const result = await customerService.getById(id);
            if (result.isFailure) {
                setError(result.error);
            }
            return result;
        } finally {
            setLoading(false);
        }
    }, []);

    const create = useCallback(async (customer: CreateCustomerDto): Promise<Result<Customer>> => {
        setLoading(true);
        setError(null);
        try {
            const result = await customerService.create(customer);
            if (result.isFailure) {
                setError(result.error);
            }
            return result;
        } finally {
            setLoading(false);
        }
    }, []);

    const update = useCallback(async (id: string, customer: UpdateCustomerDto): Promise<Result<Customer>> => {
        setLoading(true);
        setError(null);
        try {
            const result = await customerService.update(id, customer);
            if (result.isFailure) {
                setError(result.error);
            }
            return result;
        } finally {
            setLoading(false);
        }
    }, []);

    const remove = useCallback(async (id: string): Promise<Result<void>> => {
        setLoading(true);
        setError(null);
        try {
            const result = await customerService.delete(id);
            if (result.isFailure) {
                setError(result.error);
            }
            return result;
        } finally {
            setLoading(false);
        }
    }, []);

    return {
        loading,
        error,
        getAll,
        getById,
        create,
        update,
        remove
    };
}; 