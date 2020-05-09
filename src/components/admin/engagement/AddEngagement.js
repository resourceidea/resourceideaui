import React, { Component } from "react";
import AdminAddNewActionBar from "../../common/actionbar/AdminAddNewActionBard";

class AddEngagement extends Component {
    render() {
        return (
            <div className="text-justify">
                <AdminAddNewActionBar
                    page='engagement'
                    dashboardLink='/admin/engagements'
                    listLink='/admin/engagements/list' />
                <form>
                    <div className='card'>
                        <div className='card-body bg-light'>
                            <div className='row'>
                                <div className='col-6'>
                                    <div className='form-group'>
                                        <label for='title'>Engagement</label>
                                        <input type='text' className='form-control' id='title' placeholder='Engagement' />
                                    </div>
                                </div>
                                <div className='col-6'>
                                    <div className='form-group'>
                                        <label for='plannedStartDate'>Planned start date</label>
                                        <input type='text' className='form-control' id='plannedStartDate' placeholder='Planned start date' />
                                    </div>
                                </div>
                            </div>
                            <div className='row'>
                                <div className='col-6'>
                                    <div className='form-group'>
                                        <label for='partner'>Partner</label>
                                        <input type='text' className='form-control' id='partner' placeholder='Partner name' />
                                    </div>
                                </div>
                                <div className='col-6'>
                                    <div className='form-group'>
                                        <label for='manager'>Manager</label>
                                        <input type='text' className='form-control' id='manager' placeholder='Manager name' />
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

export default AddEngagement;