declare var angular: any;
declare var moment: any;

module BusSchedApp {
    interface IRouteStop {
        RouteId: number;
        NextStops: Array<any>;
    }

    interface IStop {
        Id: number;
        RouteStops: Array<IRouteStop>;
    }

    interface INextRouteStop {
        routeId: number;
        nextStops: Array<string>;
    }

    interface INextStop {
        id: number;
        routeStops: Array<INextRouteStop>;
    }

    interface IBusSchedService {
        getStops(stopId: number): any;
    }

    export class BusSchedService implements IBusSchedService {
        public static inlineAnnotatedConstructor($http: any) {
            return new BusSchedService($http);
        }

        constructor(private $http: any) {
        }

        public getStops(stopId: number) {
            return this.$http.get(`/api/bussched/${stopId}`);
        }
    }

    export class BusSchedCtrl {
        public static inlineAnnotatedConstructor(busSchedService: IBusSchedService, $q: any, $interval: any) {
            return new BusSchedCtrl(busSchedService, $q, $interval);
        }

        public started: boolean = false;
        private qInterval: any;
        public nextStops: Array<INextStop>;

        constructor(
            private busSchedService: IBusSchedService,
            private $q: any,
            private $interval: any
        ) {
        }

        public toggle() {
            if (!this.started) {
                this.getNextStops();
                this.qInterval = this.$interval(this.getNextStops, 60000);
            } else {
                this.$interval.cancel(this.qInterval);
                this.nextStops = [];
            }

            this.started = !this.started;
        }

        public getNextStops: () => void = () => {
            this.$q.all([this.busSchedService.getStops(1), this.busSchedService.getStops(2)])
                .then(responses => {
                    this.nextStops = responses.map(x => this.calculateTimeUntilNextStop(x.data));
                })
        }

        private calculateTimeUntilNextStop(stop: IStop): INextStop {
            var routeStops = stop.RouteStops.map(routeStop => {
                return {
                    routeId: routeStop.RouteId,
                    nextStops: routeStop.NextStops.map(nextStop => moment.duration(moment(nextStop).diff(moment())).minutes() + ' mins')
                } as INextRouteStop;
            });

            return {
                id: stop.Id,
                routeStops: routeStops
            };
        }
    }
}

var busSchedApp = angular.module('busSchedApp', []);
busSchedApp.factory('busSchedService', ['$http', BusSchedApp.BusSchedService.inlineAnnotatedConstructor]);
busSchedApp.controller('BusSchedCtrl', ['busSchedService', '$q', '$interval', BusSchedApp.BusSchedCtrl.inlineAnnotatedConstructor]);