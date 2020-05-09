import React, { Component } from "react";
import AdminAddNewActionBar from "../../common/actionbar/AdminAddNewActionBard";

class AddLinesOfService extends Component {
    render() {
        return (
            <div className="text-left">
                <AdminAddNewActionBar
                    page='line of service'
                    dashboardLink='/admin/lines-of-service'
                    listLink='/admin/lines-of-service/list' />
                <form>
                    <div className='card'>
                        <div className='card-body bg-light'>
                            <div className='row'>
                                <div className='col-6'>
                                    <div className='form-group'>
                                        <label for='lineOfServiceName'>Name</label>
                                        <input type='text' className='form-control' id='lineOfServiceName' placeholder='Line of service' />
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

export default AddLinesOfService;