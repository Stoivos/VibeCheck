import { useCrowdHub } from "../Hooks/useCrowdHub";

function Start() {

    const { crowd } = useCrowdHub();

    return (
        <div>
            <h1>Vibecheck</h1>

            <p>Hello world!</p>

            <pre>{JSON.stringify(crowd, null, 2)}</pre>
        </div>
    );
}

export default Start;