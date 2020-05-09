import React, { Component } from "react";
import AdminAddNewActionBar from "../../common/actionbar/AdminAddNewActionBard";

class AddEmployee extends Component {
    render() {
        return (
            <div className='text-left'>
                <AdminAddNewActionBar
                    page='employee'
                    dashboardLink='/admin/employees'
                    listLink='/admin/employees/list' />
                <form>
                    <div className='row'>
                        <div className='col-6'>
                            <div className='form-group'>
                                <label for='email'>Email</label>
                                <input type='text' className='form-control' id='email' placeholder='Email' />
                            </div>
                        </div>
                    </div>
                    <div className='row'>
                        <div className='col-6'>
                            <div className='form-group'>
                                <label for='password'>Password</label>
                                <input type='password' className='form-control' id='password' placeholder='Password' />
                            </div>
                        </div>
                    </div>
                    <hr />
                    <div className='card'>
                        <div className='card-body bg-light'>
                            <div className='row'>
                                <div className='col-6'>
                                    <div className='form-group'>
                                        <label for='firstName'>First name</label>
                                        <input type='text' className='form-control' id='firstName' placeholder='First name' />
                                    </div>
                                </div>
                                <div className='col-6'>
                                    <div className='form-group'>
                                        <label for='lastName'>Last name</label>
                                        <input type='text' className='form-control' id='lastName' placeholder='Last name' />
                                    </div>
                                </div>
                            </div>
                            <div className='row'>
                                <div className='col-6'>
                                    <div className='form-group'>
                                        <label for='phoneNumber'>Phone number</label>
                                        <input type='text' className='form-control' id='phoneNumber' placeholder='Phone number' />
                                    </div>
                                </div>
                            </div>
                            <input type='submit' value='Save' className='btn btn-danger btn-sm' />
                        </div>
                    </div>
                </form>
            </div>
        );
    }
}

export default AddEmployee;
