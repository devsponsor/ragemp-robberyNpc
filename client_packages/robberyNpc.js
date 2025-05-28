class RobberyNPCEnum {
    constructor(id, name, ped) {
        this.id = id;
        this.name = name;
        this.ped = ped;
        this.state = 0;
    }

    setRobberyState(arr_state) {
        this.state = arr_state;

        if (this.state === 1) {
            this.ped.taskHandsUp(-1, this.ped.handle, -1, true);
        } else {
            this.ped.clearTasks();
        }
    }
}

const robbery_npc = [];

mp.events.add("render", () => {
    let index = -1;
    let localPlayer = mp.players.local;

    for (let ped of robbery_npc) {
        // if (mp.game.player.isFreeAiming()) {
        //     mp.console.logInfo(`${JSON.stringify(localPlayer.aimTarget)}`);
        //     // let entityAimingAt = localPlayer.getEntityAimingAt();
        //     // if (entityAimingAt && entityAimingAt.handle === ped.pedHandle) {
        //     //     // игрок целится в этого NPC
        //     //     // делай что нужно
        //     //     mp.console.logInfo(`index setted`);
        //     //     index = ped.id;
        //     // }
        // }
        let isAiming = mp.game.player.isFreeAiming();

        if (isAiming) {
            let targetPed = getTargetedPed();
            if (targetPed !== null) {
                if (targetPed.handle === ped.ped.handle) {
                    mp.console.logInfo(`index setted`);
                    index = ped.id;
                }
            }
        }
    }

    if (mp.players.local.getVariable("Player_Aiming_To") !== index) {
        mp.console.logInfo(`sended to server`);
        mp.events.callRemote("Players_Aiming_To", index);
    }
});

function getTargetedPed() {
    // Получаем позицию камеры
    const cameraPos = mp.players.local.getBoneCoords(12844, 0, 0, 0);

    // Получаем направление, в котором смотрит камера
    const gameplayCamRot = mp.game.cam.getGameplayCamRot(2);
    const direction = rotationToDirection(gameplayCamRot);

    // Определяем конечную точку луча
    const farAway = new mp.Vector3(
        cameraPos.x + direction.x * 200,
        cameraPos.y + direction.y * 200,
        cameraPos.z + direction.z * 200
    );

    // Выполняем raycast
    const result = mp.raycasting.testPointToPoint(cameraPos, farAway, mp.players.local.handle, -1);

    // Проверяем результат
    if (result && result.entity && mp.peds.exists(result.entity)) {
        return result.entity; // Возвращаем педа
    }

    return null;
}

function rotationToDirection(rotation) {
    const z = rotation.z * 0.0174532924; // π/180
    const x = rotation.x * 0.0174532924;
    const num = Math.abs(Math.cos(x));

    return new mp.Vector3(
        -Math.sin(z) * num,
        Math.cos(z) * num,
        Math.sin(x)
    );
}

mp.events.add("CreateRobberyNPC", (name, model, position, heading, id) => {
    createRobberyNPC(name, model, position, heading, id)
});

mp.events.add("SetRobberyState", (name, state) => {
    setRobberyState(name, state)
});

function createRobberyNPC(name, model, position, heading, id) {
    const freeroamHash = mp.game.joaat(model);
    const temp_ped = new mp.peds.new(freeroamHash, new mp.Vector3(position), heading, 0);

    robbery_npc.push(new RobberyNPCEnum(id, name, temp_ped));
}

function setRobberyState(name, state) {
    robbery_npc.forEach(ped => {
        if (ped.name === name) {
            ped.setRobberyState(state);
        }
    });
}

function unixTimeNow() {
    return Math.floor(Date.now() / 1000);
}

let heistBlips = new Map();

mp.events.add('createHeistBizMark', (position, id) => {
    if (heistBlips.has(id)) {
        heistBlips.get(id).destroy();
        heistBlips.delete(id);
    }

    const blip = mp.blips.new(470, position, {
        name: 'Ограбление',
        scale: 1,
        color: 5,
        alpha: 255,
        drawDistance: 100,
        shortRange: false,
        rotation: 0,
        dimension: 0,
    });

    heistBlips.set(id, blip);
});

mp.events.add('deleteHeistBizMark', id => {
    const blip = heistBlips.get(id);
    if (blip) {
        blip.destroy();
        heistBlips.delete(id);
    }
});
