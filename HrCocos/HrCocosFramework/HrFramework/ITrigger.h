#ifndef _I_TRIGGER_H_
#define _I_TRIGGER_H_

#include "HrFramework/HrFrameworkPrerequisite.h"

namespace Hr
{
	class ITrigger
	{
	public:
		virtual ~ITrigger() {};

		virtual void Trigger() = 0;
	};
}

#endif

