import React, { useState } from 'react';
import { Customer } from '../types/customer';
import { CustomerList } from '../components/CustomerList';
import { CustomerForm } from '../components/CustomerForm';

export const CustomersPage: React.FC = () => {
    const [selectedCustomer, setSelectedCustomer] = useState<Customer | undefined>();
    const [showForm, setShowForm] = useState(false);

    const handleCreateClick = () => {
        setSelectedCustomer(undefined);
        setShowForm(true);
    };

    const handleEditClick = (customer: Customer) => {
        setSelectedCustomer(customer);
        setShowForm(true);
    };

    const handleFormSuccess = () => {
        setShowForm(false);
        setSelectedCustomer(undefined);
    };

    const handleFormCancel = () => {
        setShowForm(false);
        setSelectedCustomer(undefined);
    };

    return (
        <div className="customers-page">
            <div className="page-header">
                <h1>Customer Management</h1>
                {!showForm && (
                    <button onClick={handleCreateClick}>
                        Create New Customer
                    </button>
                )}
            </div>

            {showForm ? (
                <CustomerForm
                    customer={selectedCustomer}
                    onSuccess={handleFormSuccess}
                    onCancel={handleFormCancel}
                />
            ) : (
                <CustomerList onEdit={handleEditClick} />
            )}
        </div>
    );
}; 