import React, { useState, useEffect } from 'react';
import { Result } from 'functional-sharp';
import { Customer, CreateCustomerDto, UpdateCustomerDto } from '../types/customer';
import { useCustomer } from '../hooks/useCustomer';

interface CustomerFormProps {
    customer?: Customer;
    onSuccess?: () => void;
    onCancel?: () => void;
}

export const CustomerForm: React.FC<CustomerFormProps> = ({ customer, onSuccess, onCancel }) => {
    const { loading, error, create, update } = useCustomer();
    const [formData, setFormData] = useState<CreateCustomerDto>({
        companyName: '',
        vatNumber: '',
        email: '',
        address: {
            street: '',
            city: '',
            postalCode: '',
            country: ''
        }
    });

    useEffect(() => {
        if (customer) {
            setFormData({
                companyName: customer.companyName,
                vatNumber: customer.vatNumber,
                email: customer.email,
                address: customer.address
            });
        }
    }, [customer]);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        const result = customer
            ? await update(customer.id, formData)
            : await create(formData);

        result.match(
            () => onSuccess?.(),
            (err) => console.error(err)
        );
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        if (name.startsWith('address.')) {
            const field = name.split('.')[1];
            setFormData(prev => ({
                ...prev,
                address: {
                    ...prev.address,
                    [field]: value
                }
            }));
        } else {
            setFormData(prev => ({
                ...prev,
                [name]: value
            }));
        }
    };

    return (
        <form onSubmit={handleSubmit} className="customer-form">
            <h2>{customer ? 'Edit Customer' : 'Create Customer'}</h2>

            {error && <div className="error">{error}</div>}

            <div className="form-group">
                <label htmlFor="companyName">Company Name</label>
                <input
                    type="text"
                    id="companyName"
                    name="companyName"
                    value={formData.companyName}
                    onChange={handleChange}
                    required
                />
            </div>

            <div className="form-group">
                <label htmlFor="vatNumber">VAT Number</label>
                <input
                    type="text"
                    id="vatNumber"
                    name="vatNumber"
                    value={formData.vatNumber}
                    onChange={handleChange}
                    required
                />
            </div>

            <div className="form-group">
                <label htmlFor="email">Email</label>
                <input
                    type="email"
                    id="email"
                    name="email"
                    value={formData.email}
                    onChange={handleChange}
                    required
                />
            </div>

            <fieldset>
                <legend>Address</legend>

                <div className="form-group">
                    <label htmlFor="address.street">Street</label>
                    <input
                        type="text"
                        id="address.street"
                        name="address.street"
                        value={formData.address.street}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div className="form-group">
                    <label htmlFor="address.city">City</label>
                    <input
                        type="text"
                        id="address.city"
                        name="address.city"
                        value={formData.address.city}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div className="form-group">
                    <label htmlFor="address.postalCode">Postal Code</label>
                    <input
                        type="text"
                        id="address.postalCode"
                        name="address.postalCode"
                        value={formData.address.postalCode}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div className="form-group">
                    <label htmlFor="address.country">Country</label>
                    <input
                        type="text"
                        id="address.country"
                        name="address.country"
                        value={formData.address.country}
                        onChange={handleChange}
                        required
                    />
                </div>
            </fieldset>

            <div className="form-actions">
                <button type="submit" disabled={loading}>
                    {loading ? 'Saving...' : customer ? 'Update' : 'Create'}
                </button>
                <button type="button" onClick={onCancel}>
                    Cancel
                </button>
            </div>
        </form>
    );
}; 