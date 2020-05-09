import React, { Component } from "react";
import AdminAddNewActionBar from "../../common/actionbar/AdminAddNewActionBard";

class AddDepartment extends Component {
    render() {
        return (
            <div className="text-left">
                <AdminAddNewActionBar
                    page='department'
                    dashboardLink='/admin/departments'
                    listLink='/admin/departments/list' />
                <form>
                    <div className='card'>
                        <div className='card-body bg-light'>
                            <div className='row'>
                                <div className='col-6'>
                                    <div className='form-group'>
                                        <label for='departmentName'>Name</label>
                                        <input type='text' className='form-control' id='departmentName' placeholder='Department name' />
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

export default AddDepartment;