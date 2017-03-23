#ifndef _HR_COMMAND_H_
#define _HR_COMMAND_H_

#include "HrFramework/ITrigger.h"

namespace Hr
{
	class HrCommand : public ITrigger
	{
	public:
		HrCommand() {};
		virtual ~HrCommand() {};

		virtual void Execute() = 0;
	};
}

#endif
