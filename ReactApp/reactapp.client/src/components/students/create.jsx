import { useState, useEffect } from 'react'
import { Button, Form } from 'semantic-ui-css'

const PostData = () => {
}

const Create = () => {
    const [FullName, SetFullName] = useState('')
    const [Address, SetAddress] = useState('')
    const [Dob, SetDob] = useState()
    const [PhoneNumber, SetPhoneNumber] = useState()

    return (
        <div>
            abc
            <Form className="create-form">
                <Form.Field>
                    <label>Full name</label>
                    <input id="FullName" name="FullName" onChange={(e) => SetFullName(e.target.value)} required />
                </Form.Field>
                <Form.Field>
                    <label>Date of birth</label>
                    <input type="date" id="Dob" name="Dob" onChange={(e) => SetDob(e.target.value)} required />
                </Form.Field>
                <Form.Field>
                    <label>Phone number</label>
                    <input type="number" id="PhoneNumber" name="PhoneNumber" onChange={(e) => SetPhoneNumber(e.target.value)} required />
                </Form.Field>
                <Form.Field>
                    <label>Address</label>
                    <input type="text" id="Address" name="Address" onChange={(e) => SetAddress(e.target.value)} required />
                </Form.Field>
                <Button type='submit' onClick={PostData}>Submit</Button>
            </Form>
        </div>
    )
}

export default Create;
