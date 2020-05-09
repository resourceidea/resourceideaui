import React, { Component } from "react";
import AdminAddNewActionBar from "../../common/actionbar/AdminAddNewActionBard";

class AddClientIndustry extends Component {
    render() {
        return (
            <div className="text-left">
                <AdminAddNewActionBar
                    page='client industry'
                    dashboardLink='/admin/client-industries'
                    listLink='/admin/client-industries/list' />
                <form>
                    <div className='card'>
                        <div className='card-body bg-light'>
                            <div className='row'>
                                <div className='col-6'>
                                    <div className='form-group'>
                                        <label for='clientIndustryName'>Name</label>
                                        <input type='text' className='form-control' id='clientIndustryName' placeholder='Client industry name' />
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

export default AddClientIndustry;