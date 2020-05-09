import React, { Component } from "react";
import AdminAddNewActionBar from "../../common/actionbar/AdminAddNewActionBard";

class AddJobPosition extends Component {
    render() {
        return (
            <div className="text-left">
                <AdminAddNewActionBar
                    page='job position'
                    dashboardLink='/admin/job-positions'
                    listLink='/admin/job-positions/list' />
                <form>
                    <div className='card'>
                        <div className='card-body bg-light'>
                            <div className='row'>
                                <div className='col-6'>
                                    <div className='form-group'>
                                        <label for='title'>Title</label>
                                        <input type='text' className='form-control' id='title' placeholder='Job position' />
                                    </div>
                                </div>
                                <div className='col-6'>
                                    <div className='form-group'>
                                        <label for='department'>Department</label>
                                        <input type='text' className='form-control' id='department' placeholder='Department' />
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

export default AddJobPosition;