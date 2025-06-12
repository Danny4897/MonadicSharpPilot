import React, { useEffect, useState } from 'react';
import { Result } from 'functional-sharp';
import { Customer } from '../types/customer';
import { useCustomer } from '../hooks/useCustomer';

export const CustomerList: React.FC = () => {
    const { loading, error, getAll } = useCustomer();
    const [customers, setCustomers] = useState<Customer[]>([]);

    useEffect(() => {
        const fetchCustomers = async () => {
            const result = await getAll();
            result.match(
                (data) => setCustomers(data),
                (err) => console.error(err)
            );
        };

        fetchCustomers();
    }, [getAll]);

    if (loading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>Error: {error}</div>;
    }

    return (
        <div className="customer-list">
            <h2>Customers</h2>
            <table>
                <thead>
                    <tr>
                        <th>Company Name</th>
                        <th>VAT Number</th>
                        <th>Email</th>
                        <th>Status</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {customers.map((customer) => (
                        <tr key={customer.id}>
                            <td>{customer.companyName}</td>
                            <td>{customer.vatNumber}</td>
                            <td>{customer.email}</td>
                            <td>{customer.isActive ? 'Active' : 'Inactive'}</td>
                            <td>
                                <button onClick={() => {/* TODO: Implement edit */ }}>
                                    Edit
                                </button>
                                <button onClick={() => {/* TODO: Implement delete */ }}>
                                    Delete
                                </button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}; 